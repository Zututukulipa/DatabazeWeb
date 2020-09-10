CREATE OR REPLACE PACKAGE PKG_USER AS
    -- ||| Aliasy
    SUBTYPE T_ID IS USERS.USER_ID%TYPE;
    SUBTYPE T_USERNAME IS USERS.USERNAME%TYPE;
    SUBTYPE T_PASSWORD IS USERS.PASSWORD%TYPE;
    SUBTYPE T_FIRST_NAME IS USERS.FIRST_NAME%TYPE;
    SUBTYPE T_MIDDLE_NAME IS USERS.MIDDLE_NAME%TYPE;
    SUBTYPE T_LAST_NAME IS USERS.LAST_NAME%TYPE;
    SUBTYPE T_EMAIL IS USERS.EMAIL%TYPE;
    SUBTYPE T_STATUS IS USERS.STATUS_ID%TYPE;
    SUBTYPE T_ADMIN IS USERS.ADMIN%TYPE;
    SUBTYPE T_PROFILE_IMAGE IS USERS.PROFILE_IMAGE%TYPE;

    -- ||| FUNKCE
    -- Funkce pro vytvoření nového uživatele
    -- @return ID nového uživatele
    FUNCTION NEW(p_username T_USERNAME, p_password T_PASSWORD,
                 p_firstname T_FIRST_NAME, p_middlename T_MIDDLE_NAME, p_lastname T_LAST_NAME,
                 p_email T_EMAIL, p_status T_STATUS) RETURN T_ID;

    -- Funkce pro přihlášení uživatele na základě jeho přihlašovacích údajů
    -- @return ID přihlášeného uživatele při přihlašení, jinak NULL
    FUNCTION LOGIN(p_username T_USERNAME, p_password T_PASSWORD) RETURN T_ID;

    -- Funkce vrací záznam o uživateli se vstupním ID
    -- @return cursor
    FUNCTION GET_USER(p_user_id T_ID) RETURN SYS_REFCURSOR;

    -- Funkce vrací všechny záznamy o uživatelých
    -- @return cursor
    FUNCTION GET_ALL RETURN SYS_REFCURSOR;


    -- ||| PROCEDURY
    -- Procedura pro změnu stavu uživatele (0: Neaktivní, 1: Aktivní)
    PROCEDURE UPDATE_STATUS(p_user_id T_ID, p_status T_STATUS);

    -- Procedura pro změnu přihlašovacích údajů uživatele
    PROCEDURE UPDATE_LOGIN(p_user_id T_ID, p_username T_USERNAME, p_password T_PASSWORD);

    -- Procedura pro úpravu ostatních informací o uživateli
    PROCEDURE UPDATE_DETAILS(p_user_id T_ID,
                             p_firstname T_FIRST_NAME, p_middlename T_MIDDLE_NAME, p_lastname T_LAST_NAME,
                             p_email T_EMAIL);

    -- Procedura pro přídání/odebrání administrátorské role
    PROCEDURE UPDATE_ADMIN(p_user_id T_ID, p_admin T_ADMIN);

    -- Procedura pro změnu profilového obrázku
    PROCEDURE UPDATE_PROFILE_IMAGE(p_user_id T_ID, p_profile_image T_PROFILE_IMAGE);
END;



CREATE OR REPLACE PACKAGE BODY PKG_USER AS
    FUNCTION VALID_EMAIL(p_email T_EMAIL) RETURN BOOLEAN AS
    BEGIN
        RETURN REGEXP_LIKE(p_email, '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,10}$');
    END;

    FUNCTION VALID_LOGIN(p_username T_USERNAME, p_password T_PASSWORD) RETURN BOOLEAN AS
        v_count NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_count FROM USERS WHERE USERNAME = p_username AND PASSWORD = p_password;
        RETURN (v_count = 1);
    END;

    FUNCTION NEW(p_username T_USERNAME, p_password T_PASSWORD,
                 p_firstname T_FIRST_NAME, p_middlename T_MIDDLE_NAME, p_lastname T_LAST_NAME,
                 p_email T_EMAIL, p_status T_STATUS) RETURN T_ID IS

        v_id T_ID;
    BEGIN
        -- validace e-mailu
        IF NOT VALID_EMAIL(p_email) THEN
            RAISE_APPLICATION_ERROR(-20001, 'Can not create new user! Invalid e-mail! [' || p_email || ']');
        end if;

        -- vytvoření nového uživatele
        INSERT INTO USERS(USERNAME, PASSWORD, FIRST_NAME, MIDDLE_NAME, LAST_NAME, EMAIL, STATUS_ID)
        VALUES (p_username, p_password, p_firstname, p_middlename, p_lastname, p_email, p_status)
        RETURNING USER_ID INTO v_id;
        COMMIT;
        RETURN v_id;
    END;

    FUNCTION LOGIN(p_username T_USERNAME, p_password T_PASSWORD) RETURN T_ID IS
        v_id     T_ID;
        v_status T_STATUS;
    BEGIN
        IF NOT VALID_LOGIN(p_username, p_password) THEN
            RETURN NULL;
        END IF;

        SELECT USER_ID, STATUS_ID INTO v_id, v_status FROM USERS WHERE USERNAME = p_username AND PASSWORD = p_password;

        IF (v_status != 1) THEN
            RETURN NULL;
        END IF;

        RETURN v_id;
    END;

    FUNCTION GET_USER(p_user_id T_ID) RETURN SYS_REFCURSOR IS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_USERS WHERE USER_ID = p_user_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR IS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_USERS;
        RETURN v_cursor;
    END;

    PROCEDURE UPDATE_STATUS(p_user_id T_ID, p_status T_STATUS) IS
    BEGIN
        UPDATE USERS SET STATUS_ID = p_status WHERE USER_ID = p_user_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_LOGIN(p_user_id T_ID, p_username T_USERNAME, p_password T_PASSWORD) IS
    BEGIN
        UPDATE USERS SET USERNAME = p_username, PASSWORD = p_password WHERE USER_ID = p_user_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_DETAILS(p_user_id T_ID, p_firstname T_FIRST_NAME, p_middlename T_MIDDLE_NAME,
                             p_lastname T_LAST_NAME,
                             p_email T_EMAIL) IS
    BEGIN
        -- validace e-mailu
        IF NOT VALID_EMAIL(p_email) THEN
            RAISE_APPLICATION_ERROR(-20001, 'Can not udate user details! Invalid e-mail! [' || p_email || ']');
        END IF;

        UPDATE USERS
        SET FIRST_NAME  = p_firstname,
            MIDDLE_NAME = p_middlename,
            LAST_NAME   = p_lastname,
            EMAIL       = p_email
        WHERE USER_ID = p_user_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_ADMIN(p_user_id T_ID, p_admin T_ADMIN) IS
    BEGIN
        UPDATE USERS SET ADMIN = p_admin WHERE USER_ID = p_user_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_PROFILE_IMAGE(p_user_id T_ID, p_profile_image T_PROFILE_IMAGE) IS
    BEGIN
        UPDATE USERS SET PROFILE_IMAGE = p_profile_image WHERE USER_ID = p_user_id;
        COMMIT;
    END;
END;

