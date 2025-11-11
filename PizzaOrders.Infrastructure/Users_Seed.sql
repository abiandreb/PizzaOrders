-- --- Roles ---
IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE Id = 1)
BEGIN
INSERT INTO [Roles] (Id, [Name], NormalizedName)
VALUES (1, 'Admin', 'ADMIN');
END

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE Id = 2)
BEGIN
INSERT INTO [Roles] (Id, [Name], NormalizedName)
VALUES (2, 'User', 'USER');
END

-- --- Users ---
IF NOT EXISTS (SELECT 1 FROM [Users] WHERE Id = 1)
BEGIN
INSERT INTO [Users]
(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, IsGuest, Address, SecurityStamp, PasswordHash,
 AccessFailedCount, ConcurrencyStamp, LockoutEnabled, PhoneNumberConfirmed, TwoFactorEnabled)
VALUES
    (1, 'admin@test.com', 'ADMIN@TEST.COM', 'admin@test.com', 'ADMIN@TEST.COM', 1, 0, 'Admin HQ', 'STATIC-SEC-1', 'STATIC_HASH_1',
    0, NEWID(), 0, 0, 0);
END

IF NOT EXISTS (SELECT 1 FROM [Users] WHERE Id = 2)
BEGIN
INSERT INTO [Users]
(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, IsGuest, Address, SecurityStamp, PasswordHash,
 AccessFailedCount, ConcurrencyStamp, LockoutEnabled, PhoneNumberConfirmed, TwoFactorEnabled)
VALUES
    (2, 'user@test.com', 'USER@TEST.COM', 'user@test.com', 'USER@TEST.COM', 1, 0, 'Main St 1', 'STATIC-SEC-2', 'STATIC_HASH_2',
    0, NEWID(), 0, 0, 0);
END

IF NOT EXISTS (SELECT 1 FROM [Users] WHERE Id = 3)
BEGIN
INSERT INTO [Users]
(Id, UserName, NormalizedUserName, IsGuest, SecurityStamp, PasswordHash,
 AccessFailedCount, ConcurrencyStamp, LockoutEnabled, PhoneNumberConfirmed, TwoFactorEnabled)
VALUES
    (3, 'guest', 'GUEST', 1, 'STATIC-SEC-3', NULL,
    0, NEWID(), 0, 0, 0);
END

-- --- UserRoles ---
IF NOT EXISTS (SELECT 1 FROM [UserRoles] WHERE Id = 1)
BEGIN
INSERT INTO [UserRoles] (Id, UserId, RoleId)
VALUES (1, 1, 1);  -- admin
END

IF NOT EXISTS (SELECT 1 FROM [UserRoles] WHERE Id = 2)
BEGIN
INSERT INTO [UserRoles] (Id, UserId, RoleId)
VALUES (2, 2, 2);  -- normal user
END
