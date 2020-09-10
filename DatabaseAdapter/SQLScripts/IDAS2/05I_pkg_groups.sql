CREATE OR REPLACE PACKAGE PKG_GROUP AS
    SUBTYPE T_ID IS GROUPS.GROUP_ID%TYPE;
    SUBTYPE T_NAME IS GROUPS.NAME%TYPE;
    SUBTYPE T_MAX_CAPACITY IS GROUPS.MAX_CAPACITY%TYPE;
    SUBTYPE T_ACTUAL_CAPACITY IS GROUPS.ACTUAL_CAPACITY%TYPE;

    FUNCTION NEW(p_teacher_id PKG_TEACHER.T_ID, p_name T_NAME, p_max_capacity T_MAX_CAPACITY) RETURN T_ID;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_STUDENTS(p_group_id T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_COURSES(p_group_id T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_TEACHER(p_teacher_id PKG_TEACHER.T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_COURSES(p_course_id PKG_COURSE.T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_STUDENT(p_student_id PKG_STUDENT.T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_ID(p_group_id T_ID) RETURN SYS_REFCURSOR;

    PROCEDURE UPDATE_TEACHER(p_group_id T_ID, p_teacher_id PKG_TEACHER.T_ID);

    PROCEDURE UPDATE_NAME(p_group_id T_ID, p_name T_NAME);

    PROCEDURE UPDATE_MAX_CAPACITY(p_group_id T_ID, p_max_capacity T_MAX_CAPACITY);

    PROCEDURE UPDATE_ACTUAL_CAPACITY(p_group_id T_ID, p_actual_capacity T_ACTUAL_CAPACITY);

    PROCEDURE REMOVE(p_group_id T_ID);

    PROCEDURE ADD_STUDENT(p_group_id T_ID, p_student_id PKG_STUDENT.T_ID);

    PROCEDURE REMOVE_STUDENT(p_group_id T_ID, p_student_id PKG_STUDENT.T_ID);

    PROCEDURE ADD_COURSE(p_group_id T_ID, p_course_id PKG_COURSE.T_ID);

    PROCEDURE REMOVE_COURSE(p_group_id T_ID, p_course PKG_COURSE.T_ID);
END;

CREATE OR REPLACE PACKAGE BODY PKG_GROUP AS
    FUNCTION CHECK_GROUP(p_group_id T_ID) RETURN BOOLEAN AS
        v_count NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_count FROM VW_GROUPS WHERE GROUP_ID = p_group_id;
        RETURN (v_count = 1);
    END;

    FUNCTION NEW(p_teacher_id PKG_TEACHER.T_ID, p_name T_NAME, p_max_capacity T_MAX_CAPACITY) RETURN T_ID AS
        v_id T_ID;
    BEGIN
        INSERT INTO GROUPS(TEACHER_ID, NAME, MAX_CAPACITY)
        VALUES (p_teacher_id, p_name, p_max_capacity)
        RETURNING GROUP_ID INTO v_id;
        COMMIT;
        RETURN v_id;
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GROUPS;
        RETURN v_cursor;
    END;

    FUNCTION GET_STUDENTS(p_group_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_STUDENTS_GROUPS WHERE GROUP_ID = p_group_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_COURSES(p_group_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GROUPS_COURSES WHERE GROUP_ID = p_group_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_TEACHER(p_teacher_id PKG_TEACHER.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GROUPS WHERE TEACHER_ID = p_teacher_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_COURSES(p_course_id PKG_COURSE.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GROUPS_COURSES WHERE COURSE_ID = p_course_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_STUDENT(p_student_id PKG_STUDENT.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_STUDENTS_GROUPS WHERE STUDENT_ID = p_student_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_ID(p_group_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GROUPS WHERE GROUP_ID = p_group_id;
        RETURN v_cursor;
    END;

    PROCEDURE UPDATE_TEACHER(p_group_id T_ID, p_teacher_id PKG_TEACHER.T_ID) AS
    BEGIN
        UPDATE GROUPS SET TEACHER_ID = p_teacher_id WHERE GROUP_ID = p_group_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_NAME(p_group_id T_ID, p_name T_NAME) AS
    BEGIN
        UPDATE GROUPS SET NAME = p_name WHERE GROUP_ID = p_group_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_MAX_CAPACITY(p_group_id T_ID, p_max_capacity T_MAX_CAPACITY) AS
        v_actual_capacity NUMBER;
    BEGIN
        SELECT ACTUAL_CAPACITY INTO v_actual_capacity FROM VW_GROUPS WHERE GROUP_ID = p_group_id;

        IF (v_actual_capacity <= p_max_capacity) THEN
            UPDATE GROUPS SET MAX_CAPACITY = p_max_capacity WHERE GROUP_ID = p_group_id;
            COMMIT;
        ELSE
            RAISE_APPLICATION_ERROR(-20001, 'Ivalid max capacity of group!');
        END IF;
    END;

    PROCEDURE UPDATE_ACTUAL_CAPACITY(p_group_id T_ID, p_actual_capacity T_ACTUAL_CAPACITY) AS
    BEGIN
        UPDATE GROUPS SET ACTUAL_CAPACITY = p_actual_capacity WHERE GROUP_ID = p_group_id;
        COMMIT;
    END;

    PROCEDURE REMOVE(p_group_id T_ID) AS
    BEGIN
        DELETE FROM GROUP_MESSAGES WHERE GROUP_ID = p_group_id;
        DELETE FROM TIMETABLES WHERE GROUP_ID = p_group_id;
        DELETE FROM STUDENTS_GROUPS WHERE GROUP_ID = p_group_id;
        DELETE FROM GROUPS WHERE GROUP_ID = p_group_id;
        -- TODO
        COMMIT;
    END;

    PROCEDURE ADD_STUDENT(p_group_id T_ID, p_student_id PKG_STUDENT.T_ID) AS
        v_free_capacity   VW_GROUPS.FREE_CAPACITY%TYPE;
        v_actual_capacity T_ACTUAL_CAPACITY;
    BEGIN
        SELECT ACTUAL_CAPACITY, FREE_CAPACITY
        INTO v_actual_capacity, v_free_capacity
        FROM VW_GROUPS
        WHERE GROUP_ID = p_group_id;

        IF (v_free_capacity > 0) THEN
            INSERT INTO STUDENTS_GROUPS(STUDENT_ID, GROUP_ID) VALUES (p_group_id, p_student_id);
            UPDATE_ACTUAL_CAPACITY(p_group_id, v_actual_capacity + 1);
            COMMIT;
        ELSE
            RAISE_APPLICATION_ERROR(-20001, 'Can not add student to group! Max capacity reached!');
        END IF;
    END;

    PROCEDURE REMOVE_STUDENT(p_group_id T_ID, p_student_id PKG_STUDENT.T_ID) AS
        v_count NUMBER;
            v_actual_capacity T_ACTUAL_CAPACITY;
    BEGIN
        SELECT COUNT(*) INTO v_count FROM STUDENTS_GROUPS WHERE GROUP_ID = p_group_id AND STUDENT_ID = p_student_id;

        IF (v_count > 0) THEN
            SELECT ACTUAL_CAPACITY INTO v_actual_capacity FROM GROUPS WHERE GROUP_ID = p_group_id;
            DELETE FROM STUDENTS_GROUPS WHERE GROUP_ID = p_group_id AND STUDENT_ID = p_student_id;
            UPDATE_ACTUAL_CAPACITY(p_group_id, v_actual_capacity - 1);
            COMMIT;
        END IF;
    END;

    PROCEDURE ADD_COURSE(p_group_id T_ID, p_course_id PKG_COURSE.T_ID) AS
    BEGIN
        INSERT INTO COURSES_GROUPS(GROUP_ID, COURSE_ID) VALUES (p_group_id, p_course_id);
        COMMIT;
    END;

    PROCEDURE REMOVE_COURSE(p_group_id T_ID, p_course PKG_COURSE.T_ID) AS
    BEGIN
        DELETE COURSES_GROUPS WHERE GROUP_ID = p_group_id AND COURSE_ID = p_course;
        COMMIT;
    END;
END;