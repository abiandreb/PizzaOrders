import { test, expect } from '@playwright/test';
import { loginAsAdmin } from './helpers';

test.describe('Admin Panel - Workflow Buttons', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/');
    await loginAsAdmin(page);
  });

  test('shows only valid next-status buttons for a Paid order', async ({ page }) => {
    await page.goto('/admin');

    // Switch to Orders tab
    await page.getByRole('button', { name: /orders/i }).click();
    await page.waitForResponse((res) => res.url().includes('/management/orders') && res.status() === 200);

    // Find an order card with "Paid" status badge
    const paidBadge = page.locator('span').filter({ hasText: 'Paid' }).first();
    const hasPaidOrder = await paidBadge.isVisible().catch(() => false);

    if (hasPaidOrder) {
      // The order card containing the Paid badge
      const orderCard = paidBadge.locator('xpath=ancestor::div[contains(@class,"rounded-xl")]').first();

      // Should have "Accepted" and "Cancel" buttons
      await expect(orderCard.getByRole('button', { name: 'Accepted' })).toBeVisible();
      await expect(orderCard.getByRole('button', { name: 'Cancel' })).toBeVisible();

      // Should NOT have other status buttons like "Preparing", "Ready", etc.
      await expect(orderCard.getByRole('button', { name: 'Preparing' })).not.toBeVisible();
      await expect(orderCard.getByRole('button', { name: 'Delivering' })).not.toBeVisible();
    }
  });

  test('terminal orders show no action buttons', async ({ page }) => {
    await page.goto('/admin');

    // Switch to Orders tab
    await page.getByRole('button', { name: /orders/i }).click();
    await page.waitForResponse((res) => res.url().includes('/management/orders') && res.status() === 200);

    // Filter to Completed orders
    await page.locator('select').selectOption('Completed');
    await page.waitForResponse((res) => res.url().includes('/management/orders') && res.status() === 200);

    // Check if there are completed orders
    const completedBadge = page.locator('span').filter({ hasText: 'Completed' }).first();
    const hasCompletedOrder = await completedBadge.isVisible().catch(() => false);

    if (hasCompletedOrder) {
      const orderCard = completedBadge.locator('xpath=ancestor::div[contains(@class,"rounded-xl")]').first();

      // The "Actions:" label should NOT be present (no buttons)
      await expect(orderCard.getByText('Actions:')).not.toBeVisible();

      // Should show a terminal label instead
      await expect(orderCard.locator('span').filter({ hasText: 'Completed' })).toBeVisible();
    }
  });

  test('clicking a workflow button updates the order status', async ({ page }) => {
    await page.goto('/admin');

    // Switch to Orders tab
    await page.getByRole('button', { name: /orders/i }).click();
    await page.waitForResponse((res) => res.url().includes('/management/orders') && res.status() === 200);

    // Filter to Paid orders
    await page.locator('select').selectOption('Paid');
    await page.waitForResponse((res) => res.url().includes('/management/orders') && res.status() === 200);

    const paidBadge = page.locator('span').filter({ hasText: 'Paid' }).first();
    const hasPaidOrder = await paidBadge.isVisible().catch(() => false);

    if (hasPaidOrder) {
      const orderCard = paidBadge.locator('xpath=ancestor::div[contains(@class,"rounded-xl")]').first();

      // Click "Accepted" button
      const acceptButton = orderCard.getByRole('button', { name: 'Accepted' });
      await acceptButton.click();

      // Wait for the API response
      await page.waitForResponse((res) => res.url().includes('/status') && res.status() === 200);

      // Expect a success toast
      await expect(page.getByText(/status updated/i)).toBeVisible({ timeout: 5000 });
    }
  });
});
