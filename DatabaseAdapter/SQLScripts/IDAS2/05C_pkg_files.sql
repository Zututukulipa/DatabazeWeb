CREATE OR REPLACE PACKAGE PKG_FILE AS
    -- ||| Aliasy
    SUBTYPE T_ID IS FILES.FILE_ID%TYPE;
    SUBTYPE T_NAME IS FILES.FILE_NAME%TYPE;
    SUBTYPE T_TYPE IS FILES.FILE_TYPE%TYPE;
    SUBTYPE T_DATA IS FILES.FILE_DATA%TYPE;
    SUBTYPE T_CREATED IS FILES.CREATED%TYPE;

    -- ||| Funkce
    FUNCTION NEW(p_user_id PKG_USER.T_ID, p_file_name T_NAME, p_file_type T_TYPE, p_file_data T_DATA) RETURN T_ID;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_ID(p_file_id T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR;

    -- ||| Procedury
    PROCEDURE REMOVE(p_file_id T_ID);
END;

CREATE OR REPLACE PACKAGE BODY PKG_FILE AS
    FUNCTION CHECK_ID(p_file_id T_ID) RETURN BOOLEAN AS
        v_count NUMBER;
        BEGIN
            SELECT COUNT(*) INTO v_count FROM FILES WHERE FILE_ID = p_file_id;
            RETURN (v_count = 1);
        END;

    FUNCTION NEW(p_user_id PKG_USER.T_ID, p_file_name T_NAME, p_file_type T_TYPE, p_file_data T_DATA) RETURN T_ID AS
        v_id T_ID;
        BEGIN
            INSERT INTO FILES(USER_ID, FILE_NAME, FILE_TYPE, FILE_DATA)
            VALUES  (p_user_id, p_file_name, p_file_type, p_file_data)
            RETURNING FILE_ID INTO v_id;
            COMMIT;
            RETURN v_id;
        END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
        BEGIN
            OPEN v_cursor FOR SELECT * FROM VW_FILES;
            RETURN v_cursor;
        END;

    FUNCTION GET_BY_ID(p_file_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
        BEGIN
            OPEN v_cursor FOR SELECT * FROM VW_FILES WHERE FILE_ID = p_file_id;
            RETURN v_cursor;
        END;

    FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR AS
        v_curosr SYS_REFCURSOR;
        BEGIN
            OPEN v_curosr FOR SELECT * FROM VW_FILES WHERE USER_ID = p_user_id;
            RETURN v_curosr;
        END;

    PROCEDURE REMOVE(p_file_id T_ID) AS
        BEGIN
            DELETE FROM FILES WHERE FILE_ID = p_file_id;
            COMMIT;
        END;
END;