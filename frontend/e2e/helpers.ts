import { type Page } from '@playwright/test';

const API_BASE = 'http://localhost:5062/api';

export async function loginAsAdmin(page: Page): Promise<string> {
  const response = await page.request.post(`${API_BASE}/auth/login-user`, {
    data: { email: 'admin@pizzaorders.com', password: 'Admin123!' },
  });
  const body = await response.json();
  const token = body.token;
  await page.evaluate((t) => localStorage.setItem('token', t), token);
  // Also store user info so ProtectedRoute allows access
  await page.evaluate(() => {
    localStorage.setItem('user', JSON.stringify({ email: 'admin@pizzaorders.com', roles: ['Admin'] }));
  });
  return token;
}

export async function loginAsUser(page: Page): Promise<string> {
  const response = await page.request.post(`${API_BASE}/auth/login-user`, {
    data: { email: 'user@pizzaorders.com', password: 'User123!' },
  });
  const body = await response.json();
  const token = body.token;
  await page.evaluate((t) => localStorage.setItem('token', t), token);
  await page.evaluate(() => {
    localStorage.setItem('user', JSON.stringify({ email: 'user@pizzaorders.com', roles: ['User'] }));
  });
  return token;
}
