import { test, expect } from '@playwright/test';
import { loginAsAdmin, loginAsUser } from './helpers';

test.describe('Order Tracking - Real-Time Updates', () => {
  test('order tracking page shows status tracker', async ({ page }) => {
    await page.goto('/');
    await loginAsUser(page);

    // Navigate to orders page
    await page.goto('/orders');
    await page.waitForLoadState('networkidle');

    // If there are orders, click the first one to go to tracking
    const orderLink = page.locator('a[href^="/orders/"]').first();
    const hasOrders = await orderLink.isVisible().catch(() => false);

    if (hasOrders) {
      await orderLink.click();
      await page.waitForLoadState('networkidle');

      // Verify the status tracker is visible
      await expect(page.getByText('Order Status')).toBeVisible();
    }
  });

  test('admin status update triggers real-time toast on tracking page', async ({ browser }) => {
    // This test uses two browser contexts: one for user, one for admin
    const userContext = await browser.newContext();
    const adminContext = await browser.newContext();

    const userPage = await userContext.newPage();
    const adminPage = await adminContext.newPage();

    try {
      // Login as user
      await userPage.goto('http://localhost:3000/');
      await loginAsUser(userPage);

      // Login as admin
      await adminPage.goto('http://localhost:3000/');
      await loginAsAdmin(adminPage);

      // Navigate user to orders page to find an order
      await userPage.goto('http://localhost:3000/orders');
      await userPage.waitForLoadState('networkidle');

      const orderLink = userPage.locator('a[href^="/orders/"]').first();
      const hasOrders = await orderLink.isVisible().catch(() => false);

      if (!hasOrders) {
        test.skip(true, 'No orders available for this test');
        return;
      }

      // Get the order ID from the link
      const href = await orderLink.getAttribute('href');
      const orderId = href?.split('/').pop();

      // Navigate user to order tracking page
      await userPage.goto(`http://localhost:3000/orders/${orderId}`);
      await userPage.waitForLoadState('networkidle');
      await expect(userPage.getByText('Order Status')).toBeVisible();

      // Navigate admin to admin panel, orders tab
      await adminPage.goto('http://localhost:3000/admin');
      await adminPage.getByRole('button', { name: /orders/i }).click();
      await adminPage.waitForResponse(
        (res) => res.url().includes('/management/orders') && res.status() === 200
      );

      // Find the same order in admin panel and check for a workflow button
      const orderCard = adminPage.locator(`text=Order #${orderId}`).locator('..').locator('..');
      const hasActionButton = await orderCard
        .getByRole('button')
        .filter({ hasNotText: /orders|products|toppings/i })
        .first()
        .isVisible()
        .catch(() => false);

      if (hasActionButton) {
        // Click the first non-Cancel workflow button
        const actionButtons = orderCard
          .getByRole('button')
          .filter({ hasNotText: /cancel|orders|products|toppings/i });

        const buttonCount = await actionButtons.count();

        if (buttonCount > 0) {
          await actionButtons.first().click();

          // Wait for the API response on admin side
          await adminPage.waitForResponse(
            (res) => res.url().includes('/status') && res.status() === 200
          );

          // On user page, expect a toast notification about status update
          await expect(
            userPage.getByText(/order status updated/i)
          ).toBeVisible({ timeout: 10000 });
        }
      }
    } finally {
      await userContext.close();
      await adminContext.close();
    }
  });
});
