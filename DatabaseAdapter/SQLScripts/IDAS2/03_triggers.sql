-- ||| TRIGGERY
CREATE OR REPLACE TRIGGER users_insert_trg
    BEFORE INSERT ON USERS
    FOR EACH ROW WHEN (new.USER_ID IS NULL)
    BEGIN
        :new.USER_ID := users_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER pmsgs_insert_trg
    BEFORE INSERT ON PRIVATE_MESSAGES
    FOR EACH ROW WHEN (new.PMSG_ID IS NULL)
    BEGIN
        :new.PMSG_ID := pmsgs_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER students_insert_trg
    BEFORE INSERT ON STUDENTS
    FOR EACH ROW WHEN (new.STUDENT_ID IS NULL)
    BEGIN
        :new.STUDENT_ID := students_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER courses_insert_trg
    BEFORE INSERT ON COURSES
    FOR EACH ROW WHEN (new.COURSE_ID IS NULL)
    BEGIN
        :new.COURSE_ID := courses_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER teachers_insert_trg
    BEFORE INSERT ON TEACHERS
    FOR EACH ROW WHEN (new.TEACHER_ID IS NULL)
    BEGIN
        :new.TEACHER_ID := teachers_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER grades_insert_trg
    BEFORE INSERT ON GRADES
    FOR EACH ROW WHEN (new.GRADE_ID IS NULL)
    BEGIN
        :new.GRADE_ID := grades_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER groups_insert_trg
    BEFORE INSERT ON GROUPS
    FOR EACH ROW WHEN (new.GROUP_ID IS NULL)
    BEGIN
        :new.GROUP_ID := groups_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER classrooms_insert_trg
    BEFORE INSERT ON CLASSROOMS
    FOR EACH ROW WHEN (new.CLASSROOM_ID IS NULL)
    BEGIN
        :new.CLASSROOM_ID := classrooms_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER gmsg_insert_trg
    BEFORE INSERT ON GROUP_MESSAGES
    FOR EACH ROW WHEN (new.GMSG_ID IS NULL)
    BEGIN
        :new.GMSG_ID := gmsgs_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER timetables_insert_trg
    BEFORE INSERT ON TIMETABLES
    FOR EACH ROW WHEN (new.TIMETABLE_ID IS NULL)
    BEGIN
        :new.TIMETABLE_ID := timetables_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER comments_insert_trg
    BEFORE INSERT ON COMMENTS
    FOR EACH ROW WHEN (new.COMMENT_ID IS NULL)
    BEGIN
        :new.COMMENT_ID := comments_id_seq.nextval;
    END;

CREATE OR REPLACE TRIGGER files_insert_trg
    BEFORE INSERT ON FILES
    FOR EACH ROW WHEN (new.FILE_ID IS NULL)
    BEGIN
        :new.FILE_ID := FILES_ID_SEQ.nextval;
    END;