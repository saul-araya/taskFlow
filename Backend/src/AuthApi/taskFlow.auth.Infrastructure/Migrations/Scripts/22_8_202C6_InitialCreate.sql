CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260823022156_InitialCreate') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'auth') THEN
            CREATE SCHEMA auth;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260823022156_InitialCreate') THEN
    CREATE TABLE auth."Users" (
        id uuid NOT NULL,
        name character varying(255) NOT NULL,
        "DisplayName" text NOT NULL,
        email character varying(255) NOT NULL,
        image_link text,
        active boolean NOT NULL,
        created_at timestamp with time zone NOT NULL,
        updated_at timestamp with time zone,
        CONSTRAINT "PK_Users" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260823022156_InitialCreate') THEN
    CREATE TABLE auth."UserProviders" (
        id uuid NOT NULL,
        user_id uuid NOT NULL,
        provider character varying(50) NOT NULL,
        provider_user_id character varying(255),
        password_hash character varying(255),
        CONSTRAINT "PK_UserProviders" PRIMARY KEY (id),
        CONSTRAINT "FK_UserProviders_Users_user_id" FOREIGN KEY (user_id) REFERENCES auth."Users" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260823022156_InitialCreate') THEN
    CREATE INDEX "IX_USER_PROVIDER_FK" ON auth."UserProviders" (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260823022156_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_USER_EMAIL" ON auth."Users" (email);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260823022156_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260823022156_InitialCreate', '10.0.11');
    END IF;
END $EF$;
COMMIT;

