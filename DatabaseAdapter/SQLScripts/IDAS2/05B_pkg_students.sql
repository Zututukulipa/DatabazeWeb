CREATE OR REPLACE PACKAGE PKG_STUDENT AS
    -- ||| ALIASY
    SUBTYPE T_ID IS STUDENTS.STUDENT_ID%TYPE;
    SUBTYPE T_YEAR IS STUDENTS.YEAR%TYPE;

    -- ||| FUNKCE
    -- Funkce pro vytvoření nového studenta na základě ID užvatele
    -- @return ID nového studenta
    FUNCTION NEW(p_user_id PKG_USER.T_ID, p_year T_YEAR DEFAULT 1) RETURN T_ID;

    -- Funkce pro vrácení dat o všech studentech
    -- @return cursor
    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    -- Funkce vrátí data o studentovi na základě studentova ID
    -- @return cursor
    FUNCTION GET_BY_ID(p_student_id T_ID) RETURN SYS_REFCURSOR;

    -- Funkce vrátí data o studentovi na základě uživatelova ID
    -- @return cursor
    FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR;

    -- Funckce pro ověření, zdali uživatel je studentem
    -- @return boolean
    FUNCTION IS_STUDENT(p_user_id PKG_USER.T_ID) RETURN BOOLEAN;

    -- ||| PROCEDURY
    -- Procedura pro úpravu ročníku studenta
    PROCEDURE UPDATE_YEAR(p_student_id T_ID, p_year T_YEAR);

END;

CREATE OR REPLACE PACKAGE BODY PKG_STUDENT AS
    FUNCTION NEW(p_user_id PKG_USER.T_ID, p_year T_YEAR DEFAULT 1) RETURN T_ID IS
        v_id T_ID;
    BEGIN
        INSERT INTO STUDENTS(USER_ID, YEAR) VALUES (p_user_id, p_year);
        COMMIT;
        SELECT STUDENT_ID INTO v_id FROM STUDENTS WHERE USER_ID = p_user_id;
        RETURN v_id;
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_STUDENTS;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_ID(p_student_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_STUDENTS WHERE STUDENT_ID = p_student_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_STUDENTS WHERE USER_ID = p_user_id;
        RETURN v_cursor;
    END;

    FUNCTION IS_STUDENT(p_user_id PKG_USER.T_ID) RETURN BOOLEAN AS
        v_count NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_count FROM STUDENTS WHERE USER_ID = p_user_id;
        RETURN (v_count >= 1);
    END;

    PROCEDURE UPDATE_YEAR(p_student_id T_ID, p_year T_YEAR) AS
    BEGIN
        UPDATE STUDENTS SET YEAR = p_year WHERE STUDENT_ID = p_student_id;
    END;
END;