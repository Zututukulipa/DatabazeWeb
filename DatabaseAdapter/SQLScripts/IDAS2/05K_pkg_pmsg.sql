CREATE OR REPLACE PACKAGE PKG_PMSG AS
    SUBTYPE T_ID IS PRIVATE_MESSAGES.PMSG_ID%TYPE;
    SUBTYPE T_CONTENT IS PRIVATE_MESSAGES.CONTENT%TYPE;
    SUBTYPE T_CREATED IS PRIVATE_MESSAGES.CREATED%TYPE;

    FUNCTION NEW(p_from_user PKG_USER.T_ID, p_to_user PKG_USER.T_ID, p_content T_CONTENT) RETURN T_ID;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_USER(p_user PKG_USER.T_ID) RETURN SYS_REFCURSOR;

    PROCEDURE REMOVE(p_id T_ID);
END;

CREATE OR REPLACE PACKAGE BODY PKG_PMSG AS
    FUNCTION NEW(p_from_user PKG_USER.T_ID, p_to_user PKG_USER.T_ID, p_content T_CONTENT) RETURN T_ID AS
        v_id T_ID;
    BEGIN
        INSERT INTO PRIVATE_MESSAGES(FROM_USER, TO_USER, CONTENT)
        VALUES (p_from_user, p_to_user, p_content)
        RETURNING PMSG_ID INTO v_id;
        COMMIT;
        RETURN v_id;
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_PRIVATE_MESSAGES;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_PRIVATE_MESSAGES WHERE MESSAGE_ID = p_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_USER(p_user PKG_USER.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_PRIVATE_MESSAGES WHERE FROM_USER_ID = p_user OR TO_USER_ID = p_user;
        RETURN v_cursor;
    END;

    PROCEDURE REMOVE(p_id T_ID) AS
    BEGIN
        DELETE PRIVATE_MESSAGES WHERE PMSG_ID = p_id;
        COMMIT;
    END;
END;