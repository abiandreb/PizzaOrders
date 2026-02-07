import { useEffect, useRef } from 'react';
import { HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';

interface OrderStatusUpdate {
  orderId: number;
  status: string;
  updatedAt: string;
}

export function useOrderSignalR(
  orderId: number | undefined,
  onStatusUpdate: (update: OrderStatusUpdate) => void
) {
  const callbackRef = useRef(onStatusUpdate);
  callbackRef.current = onStatusUpdate;

  useEffect(() => {
    if (!orderId) return;

    const token = localStorage.getItem('token');
    if (!token) return;

    const connection = new HubConnectionBuilder()
      .withUrl('/hubs/orders', {
        accessTokenFactory: () => localStorage.getItem('token') ?? '',
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build();

    let stopped = false;

    async function start() {
      try {
        await connection.start();
        if (stopped) {
          await connection.stop();
          return;
        }
        await connection.invoke('JoinOrderGroup', orderId);
      } catch (err) {
        console.error('SignalR connection failed:', err);
      }
    }

    connection.on('OrderStatusUpdated', (data: OrderStatusUpdate) => {
      callbackRef.current(data);
    });

    connection.onreconnected(async () => {
      try {
        await connection.invoke('JoinOrderGroup', orderId);
      } catch {
        // Group rejoin failed after reconnect
      }
    });

    start();

    return () => {
      stopped = true;
      if (connection.state === HubConnectionState.Connected) {
        connection.invoke('LeaveOrderGroup', orderId)
          .catch(() => {})
          .finally(() => connection.stop());
      } else {
        connection.stop();
      }
    };
  }, [orderId]);
}
