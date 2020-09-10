CREATE OR REPLACE PACKAGE PKG_GRADES AS
    SUBTYPE T_ID IS GRADES.GRADE_ID%TYPE;
    SUBTYPE T_VALUE IS GRADES.VALUE%TYPE;
    SUBTYPE T_DESCRIPTION IS GRADES.DESCRIPTION%TYPE;
    SUBTYPE T_CREATED IS GRADES.CREATED%TYPE;

    FUNCTION NEW(p_student_id PKG_STUDENT.T_ID, p_teacher_id PKG_TEACHER.T_ID, p_course_id PKG_COURSE.T_ID,
                 p_value T_VALUE, p_description T_DESCRIPTION) RETURN T_ID;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_COURSE(p_course_id PKG_COURSE.T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_TEACHER(p_teacher_id PKG_TEACHER.T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_STUDENT(p_student_id PKG_STUDENT.T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_ID(p_grade_id T_ID) RETURN SYS_REFCURSOR;

    PROCEDURE REMOVE(p_grade_id T_ID);

    PROCEDURE UPDATE_VALUE(p_grade_id T_ID, p_value T_VALUE);

    PROCEDURE UPDATE_DESCRIPTION(p_grade_id T_ID, p_description T_DESCRIPTION);

    PROCEDURE UPDATE_COURSE(p_grade_id T_ID, p_course_id PKG_COURSE.T_ID);

    PROCEDURE UPDATE_STUDENT(p_grade_id T_ID, p_student_id PKG_STUDENT.T_ID);

    PROCEDURE UPDATE_TEACHER(p_grade_id T_ID, p_teacher_id PKG_TEACHER.T_ID);
END;

CREATE OR REPLACE PACKAGE BODY PKG_GRADES AS
    FUNCTION NEW(p_student_id PKG_STUDENT.T_ID, p_teacher_id PKG_TEACHER.T_ID, p_course_id PKG_COURSE.T_ID,
                 p_value T_VALUE, p_description T_DESCRIPTION) RETURN T_ID AS
        v_id T_ID;
    BEGIN
        INSERT INTO GRADES(STUDENT_ID, TEACHER_ID, COURSE_ID, VALUE, DESCRIPTION)
        VALUES (p_student_id, p_teacher_id, p_course_id, p_value, p_description)
        RETURNING GRADE_ID INTO v_id;
        COMMIT;
        RETURN v_id;
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GRADES;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_COURSE(p_course_id PKG_COURSE.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM GRADES WHERE COURSE_ID = p_course_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_TEACHER(p_teacher_id PKG_TEACHER.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GRADES WHERE TEACHER_ID = p_teacher_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_STUDENT(p_student_id PKG_STUDENT.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GRADES WHERE STUDENT_ID = p_student_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_ID(p_grade_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GRADES WHERE GRADE_ID = p_grade_id;
        RETURN v_cursor;
    END;

    PROCEDURE REMOVE(p_grade_id T_ID) AS
    BEGIN
        DELETE GRADES WHERE GRADE_ID = p_grade_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_VALUE(p_grade_id T_ID, p_value T_VALUE) AS
    BEGIN
        UPDATE GRADES SET VALUE = p_value WHERE GRADE_ID = p_grade_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_DESCRIPTION(p_grade_id T_ID, p_description T_DESCRIPTION) AS
    BEGIN
        UPDATE GRADES SET DESCRIPTION = p_description WHERE GRADE_ID = p_grade_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_COURSE(p_grade_id T_ID, p_course_id PKG_COURSE.T_ID) AS
    BEGIN
        UPDATE GRADES SET COURSE_ID = p_course_id WHERE GRADE_ID = p_grade_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_STUDENT(p_grade_id T_ID, p_student_id PKG_STUDENT.T_ID) AS
    BEGIN
        UPDATE GRADES SET STUDENT_ID = p_student_id WHERE GRADE_ID = p_grade_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_TEACHER(p_grade_id T_ID, p_teacher_id PKG_TEACHER.T_ID) AS
    BEGIN
        UPDATE GRADES SET TEACHER_ID = p_teacher_id WHERE GRADE_ID = p_grade_id;
        COMMIT;
    END;
END;
