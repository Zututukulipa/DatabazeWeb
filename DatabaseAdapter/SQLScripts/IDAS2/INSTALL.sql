-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- TABULKY
-- *********************************************************************************************************************
-- *********************************************************************************************************************
create table USERS_STATUS
(
    STATUS_ID NUMBER        not null,
    NAME      NVARCHAR2(15) not null
)
/

create unique index USERS_STATUS_NAME_UINDEX
    on USERS_STATUS (NAME)
/

create unique index USERS_STATUS_STATUS_ID_UINDEX
    on USERS_STATUS (STATUS_ID)
/

alter table USERS_STATUS
    add constraint USERS_STATUS_PK
        primary key (STATUS_ID)
/

alter table USERS_STATUS
    add check ( STATUS_ID IN (0, 1) )
/

create table FILES
(
    FILE_ID   NUMBER               not null,
    USER_ID   NUMBER               not null,
    FILE_NAME NVARCHAR2(256)       not null,
    FILE_TYPE NVARCHAR2(5),
    FILE_DATA BLOB                 not null,
    CREATED   DATE default SYSDATE not null
)
/

create unique index FILES_FILE_ID_UINDEX
    on FILES (FILE_ID)
/

alter table FILES
    add constraint FILES_PK
        primary key (FILE_ID)
/

create table USERS
(
    USER_ID       NUMBER           not null,
    USERNAME      NVARCHAR2(50)    not null,
    PASSWORD      NVARCHAR2(256)   not null,
    FIRST_NAME    NVARCHAR2(30)    not null,
    MIDDLE_NAME   NVARCHAR2(30),
    LAST_NAME     NVARCHAR2(40)    not null,
    EMAIL         NVARCHAR2(100)   not null,
    STATUS_ID     NUMBER           not null,
    ADMIN         NUMBER default 0 not null,
    PROFILE_IMAGE NUMBER,
    constraint USERS_FILES_FK
        foreign key (PROFILE_IMAGE) references FILES,
    constraint USERS_STATUS_FK
        foreign key (STATUS_ID) references USERS_STATUS
)
/

comment on table USERS is 'Table of all users'
/

create unique index USERS_EMAIL_UINDEX
    on USERS (EMAIL)
/

create unique index USERS_USER_ID_UINDEX
    on USERS (USER_ID)
/

create unique index USERS_USERNAME_UINDEX
    on USERS (USERNAME)
/

alter table USERS
    add constraint USERS_PK
        primary key (USER_ID)
/

alter table FILES
    add constraint FILES_USERS_FK
        foreign key (USER_ID) references USERS
/

alter table USERS
    add check ( ADMIN IN (0, 1) )
/

create table COURSES
(
    COURSE_ID   NUMBER         not null,
    FULL_NAME   NVARCHAR2(50)  not null,
    SHORT_NAME  NVARCHAR2(10)  not null,
    DESCRIPTION NVARCHAR2(256) not null
)
/

create unique index COURSES_COURSE_ID_UINDEX
    on COURSES (COURSE_ID)
/

create unique index COURSES_SHORT_NAME_UINDEX
    on COURSES (SHORT_NAME)
/

alter table COURSES
    add constraint COURSES_PK
        primary key (COURSE_ID)
/

create table TEACHERS
(
    TEACHER_ID NUMBER not null,
    USER_ID    NUMBER not null,
    constraint TEACHERS_USERS_FK
        foreign key (USER_ID) references USERS
)
/

create unique index TEACHERS_TEACHER_ID_UINDEX
    on TEACHERS (TEACHER_ID)
/

create unique index TEACHERS_USER_ID_UINDEX
    on TEACHERS (USER_ID)
/

alter table TEACHERS
    add constraint TEACHERS_PK
        primary key (TEACHER_ID)
/

create table GROUPS
(
    GROUP_ID        NUMBER           not null,
    TEACHER_ID      NUMBER           not null,
    NAME            NVARCHAR2(10)    not null,
    MAX_CAPACITY    NUMBER           not null,
    ACTUAL_CAPACITY NUMBER default 0 not null,
    constraint GROUPS_TEACHERS_FK
        foreign key (TEACHER_ID) references TEACHERS
)
/

create unique index GROUPS_GROUP_ID_UINDEX
    on GROUPS (GROUP_ID)
/

create unique index GROUPS_NAME_UINDEX
    on GROUPS (NAME)
/

alter table GROUPS
    add constraint GROUPS_PK
        primary key (GROUP_ID)
/

create table STUDENTS
(
    STUDENT_ID NUMBER           not null,
    USER_ID    NUMBER           not null,
    YEAR       NUMBER default 1 not null,
    constraint STUDENTS_USERS_FK
        foreign key (USER_ID) references USERS
)
/

create unique index STUDENTS_STUDENT_ID_UINDEX
    on STUDENTS (STUDENT_ID)
/

create unique index STUDENTS_USER_ID_UINDEX
    on STUDENTS (USER_ID)
/

alter table STUDENTS
    add constraint STUDENTS_PK
        primary key (STUDENT_ID)
/

create table GRADES
(
    GRADE_ID    NUMBER               not null,
    STUDENT_ID  NUMBER               not null,
    TEACHER_ID  NUMBER               not null,
    COURSE_ID   NUMBER               not null,
    VALUE       NUMBER               not null,
    DESCRIPTION NVARCHAR2(256),
    CREATED     DATE default SYSDATE not null,
    constraint GRADES_COURSES_FK
        foreign key (COURSE_ID) references COURSES,
    constraint GRADES_STUDENTS_FK
        foreign key (STUDENT_ID) references STUDENTS,
    constraint GRADES_TEACHERS_FK
        foreign key (TEACHER_ID) references TEACHERS
)
/

create unique index GRADES_GRADE_ID_UINDEX
    on GRADES (GRADE_ID)
/

alter table GRADES
    add constraint GRADES_PK
        primary key (GRADE_ID)
/

create table CLASSROOMS
(
    CLASSROOM_ID NUMBER        not null,
    NAME         NVARCHAR2(15) not null,
    CAPACITY     NUMBER        not null
)
/

create unique index CLASSROOMS_CLASSROOM_ID_UINDEX
    on CLASSROOMS (CLASSROOM_ID)
/

alter table CLASSROOMS
    add constraint CLASSROOMS_PK
        primary key (CLASSROOM_ID)
/

create table TIMETABLES
(
    TIMETABLE_ID NUMBER not null,
    GROUP_ID     NUMBER not null,
    CLASSROOM_ID NUMBER not null,
    BEGIN        DATE   not null,
    END          DATE   not null,
    constraint TIMETABLES_CLSROOM_FK
        foreign key (CLASSROOM_ID) references CLASSROOMS,
    constraint TIMETABLES_GROUPS_FK
        foreign key (GROUP_ID) references GROUPS
)
/

create unique index TIMETABLES_TIMETABLE_ID_UINDEX
    on TIMETABLES (TIMETABLE_ID)
/

alter table TIMETABLES
    add constraint TIMETABLES_PK
        primary key (TIMETABLE_ID)
/

create table GROUP_MESSAGES
(
    GMSG_ID  NUMBER               not null,
    GROUP_ID NUMBER,
    USER_ID  NUMBER               not null,
    CONTENT  NVARCHAR2(512)       not null,
    CREATED  DATE default SYSDATE not null,
    constraint GMSG_GROUPS_FK
        foreign key (GROUP_ID) references GROUPS,
    constraint GMSG_USERS_FK
        foreign key (USER_ID) references USERS
)
/

create unique index GROUP_MESSAGES_GMSG_ID_UINDEX
    on GROUP_MESSAGES (GMSG_ID)
/

alter table GROUP_MESSAGES
    add constraint GROUP_MESSAGES_PK
        primary key (GMSG_ID)
/

create table PRIVATE_MESSAGES
(
    PMSG_ID   NUMBER               not null,
    FROM_USER NUMBER               not null,
    TO_USER   NUMBER               not null,
    CONTENT   NVARCHAR2(512)       not null,
    CREATED   DATE default SYSDATE not null,
    constraint PMSG_FROM_USR_FK
        foreign key (FROM_USER) references USERS,
    constraint PMSG_TO_USR_FK
        foreign key (TO_USER) references USERS
)
/

create unique index PRIVATE_MESSAGES_PMSG_ID_UINDEX
    on PRIVATE_MESSAGES (PMSG_ID)
/

alter table PRIVATE_MESSAGES
    add constraint PRIVATE_MESSAGES_PK
        primary key (PMSG_ID)
/

create table COMMENTS
(
    COMMENT_ID NUMBER               not null,
    USER_ID    NUMBER               not null,
    MESSAGE_ID NUMBER               not null,
    CONTENT    NVARCHAR2(256)       not null,
    CREATED    DATE default SYSDATE not null,
    constraint COMMENTS_GMSG_FK
        foreign key (MESSAGE_ID) references GROUP_MESSAGES,
    constraint "comments_users.fk"
        foreign key (USER_ID) references USERS
)
/

create unique index COMMENTS_COMMENT_ID_UINDEX
    on COMMENTS (COMMENT_ID)
/

alter table COMMENTS
    add constraint COMMENTS_PK
        primary key (COMMENT_ID)
/

create table STUDENTS_GROUPS
(
    STUDENT_ID NUMBER not null,
    GROUP_ID   NUMBER not null,
    constraint STUDENTS_GROUPS_PK
        primary key (STUDENT_ID),
    constraint SG_GROUPS_FK
        foreign key (GROUP_ID) references GROUPS,
    constraint SG_STUDENTS_FK
        foreign key (STUDENT_ID) references STUDENTS
)
/

create table COURSES_GROUPS
(
    GROUP_ID  NUMBER not null,
    COURSE_ID NUMBER not null,
    constraint COURSES_GROUPS_PK
        primary key (GROUP_ID),
    constraint CRSGRP_COURSES_FK
        foreign key (COURSE_ID) references COURSES,
    constraint CRSGRP_GROUPS_FK
        foreign key (GROUP_ID) references GROUPS
)
/

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- SEKVENCE
-- *********************************************************************************************************************
-- *********************************************************************************************************************
CREATE SEQUENCE users_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE pmsgs_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE students_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE courses_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE teachers_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE grades_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE groups_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE classrooms_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE gmsgs_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE timetables_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE comments_id_seq START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE files_id_seq START WITH 1 INCREMENT BY 1;

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- TIGGERY
-- *********************************************************************************************************************
-- *********************************************************************************************************************
CREATE OR REPLACE TRIGGER users_insert_trg
    BEFORE INSERT
    ON USERS
    FOR EACH ROW
    WHEN (new.USER_ID IS NULL)
BEGIN
    :new.USER_ID := users_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER pmsgs_insert_trg
    BEFORE INSERT
    ON PRIVATE_MESSAGES
    FOR EACH ROW
    WHEN (new.PMSG_ID IS NULL)
BEGIN
    :new.PMSG_ID := pmsgs_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER students_insert_trg
    BEFORE INSERT
    ON STUDENTS
    FOR EACH ROW
    WHEN (new.STUDENT_ID IS NULL)
BEGIN
    :new.STUDENT_ID := students_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER courses_insert_trg
    BEFORE INSERT
    ON COURSES
    FOR EACH ROW
    WHEN (new.COURSE_ID IS NULL)
BEGIN
    :new.COURSE_ID := courses_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER teachers_insert_trg
    BEFORE INSERT
    ON TEACHERS
    FOR EACH ROW
    WHEN (new.TEACHER_ID IS NULL)
BEGIN
    :new.TEACHER_ID := teachers_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER grades_insert_trg
    BEFORE INSERT
    ON GRADES
    FOR EACH ROW
    WHEN (new.GRADE_ID IS NULL)
BEGIN
    :new.GRADE_ID := grades_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER groups_insert_trg
    BEFORE INSERT
    ON GROUPS
    FOR EACH ROW
    WHEN (new.GROUP_ID IS NULL)
BEGIN
    :new.GROUP_ID := groups_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER classrooms_insert_trg
    BEFORE INSERT
    ON CLASSROOMS
    FOR EACH ROW
    WHEN (new.CLASSROOM_ID IS NULL)
BEGIN
    :new.CLASSROOM_ID := classrooms_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER gmsg_insert_trg
    BEFORE INSERT
    ON GROUP_MESSAGES
    FOR EACH ROW
    WHEN (new.GMSG_ID IS NULL)
BEGIN
    :new.GMSG_ID := gmsgs_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER timetables_insert_trg
    BEFORE INSERT
    ON TIMETABLES
    FOR EACH ROW
    WHEN (new.TIMETABLE_ID IS NULL)
BEGIN
    :new.TIMETABLE_ID := timetables_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER comments_insert_trg
    BEFORE INSERT
    ON COMMENTS
    FOR EACH ROW
    WHEN (new.COMMENT_ID IS NULL)
BEGIN
    :new.COMMENT_ID := comments_id_seq.nextval;
END;

CREATE OR REPLACE TRIGGER files_insert_trg
    BEFORE INSERT
    ON FILES
    FOR EACH ROW
    WHEN (new.FILE_ID IS NULL)
BEGIN
    :new.FILE_ID := FILES_ID_SEQ.nextval;
END;

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- POHLEDY
-- *********************************************************************************************************************
-- *********************************************************************************************************************
CREATE OR REPLACE VIEW VW_USERS_STATUS AS
SELECT *
FROM USERS_STATUS;

CREATE OR REPLACE VIEW VW_USERS AS
SELECT USERS.USER_ID,
       USERNAME,
       FIRST_NAME,
       MIDDLE_NAME,
       LAST_NAME,
       EMAIL,
       PROFILE_IMAGE     PROFILE_IMAGE_ID,
       FILES.FILE_DATA   PROFILE_IMAGE_DATA,
       USERS.STATUS_ID,
       USERS_STATUS.NAME STATUS_NAME,
       ADMIN
FROM USERS
         JOIN USERS_STATUS ON USERS.STATUS_ID = USERS_STATUS.STATUS_ID
         LEFT JOIN FILES ON USERS.PROFILE_IMAGE = FILES.FILE_ID;

CREATE OR REPLACE VIEW VW_STUDENTS AS
SELECT STUDENT_ID,
       STUDENTS.USER_ID,
       LAST_NAME || ', ' || FIRST_NAME || (CASE WHEN MIDDLE_NAME IS NOT NULL THEN ' ' || MIDDLE_NAME END) NAME,
       EMAIL,
       YEAR,
       STATUS_ID,
       STATUS_NAME
FROM STUDENTS
         JOIN VW_USERS ON VW_USERS.USER_ID = STUDENTS.USER_ID;

CREATE OR REPLACE VIEW VW_FILES AS
SELECT FILE_ID, FILES.USER_ID USER_ID, USERS.USERNAME USER_USERNAME, FIRST_NAME, FILE_TYPE, FILE_DATA, CREATED
FROM FILES
         JOIN USERS ON FILES.USER_ID = USERS.USER_ID;

CREATE OR REPLACE VIEW VW_TEACHERS AS
SELECT TEACHER_ID,
       TEACHERS.USER_ID,
       LAST_NAME || ', ' || FIRST_NAME || (CASE WHEN MIDDLE_NAME IS NOT NULL THEN ' ' || MIDDLE_NAME END) NAME,
       EMAIL,
       STATUS_ID,
       STATUS_NAME
FROM TEACHERS
         JOIN VW_USERS ON TEACHERS.USER_ID = VW_USERS.USER_ID;

CREATE OR REPLACE VIEW VW_CLASSROOMS AS
SELECT *
FROM CLASSROOMS;

CREATE OR REPLACE VIEW VW_COURSES AS
SELECT *
FROM COURSES;

CREATE OR REPLACE VIEW VW_GRADES AS
SELECT GRADE_ID,
       GRADES.STUDENT_ID,
       VW_STUDENTS.NAME      STUDENT_NAME,
       GRADES.TEACHER_ID,
       VW_TEACHERS.NAME      TEACHER_NAME,
       GRADES.COURSE_ID,
       VW_COURSES.SHORT_NAME COURSE_SHORT_NAME,
       VW_COURSES.FULL_NAME  COURSE_FULL_NAME,
       VALUE                 GRADE_VALUE,
       GRADES.DESCRIPTION    GRADE_DESCRIPTION,
       CREATED               GRADE_CREATED
FROM GRADES
         JOIN VW_STUDENTS ON GRADES.STUDENT_ID = VW_STUDENTS.STUDENT_ID
         JOIN VW_TEACHERS ON GRADES.TEACHER_ID = VW_TEACHERS.TEACHER_ID
         JOIN VW_COURSES ON GRADES.COURSE_ID = VW_COURSES.COURSE_ID;

CREATE OR REPLACE VIEW VW_GROUPS AS
SELECT GROUP_ID,
       GROUPS.TEACHER_ID,
       VW_TEACHERS.NAME                 TEACHER_NAME,
       GROUPS.NAME                      GROUP_NAME,
       MAX_CAPACITY,
       ACTUAL_CAPACITY,
       (MAX_CAPACITY - ACTUAL_CAPACITY) FREE_CAPACITY
FROM GROUPS
         JOIN VW_TEACHERS ON GROUPS.TEACHER_ID = VW_TEACHERS.TEACHER_ID;

CREATE OR REPLACE VIEW VW_GROUPS_COURSES AS
SELECT CG.GROUP_ID,
       VW_GROUPS.GROUP_NAME,
       VW_GROUPS.MAX_CAPACITY,
       VW_GROUPS.ACTUAL_CAPACITY,
       VW_GROUPS.FREE_CAPACITY,
       VW_GROUPS.TEACHER_ID,
       VW_GROUPS.TEACHER_NAME,
       CG.COURSE_ID,
       VW_COURSES.FULL_NAME   COURSE_FULL_NAME,
       VW_COURSES.SHORT_NAME  COURSE_SHORT_NAME,
       VW_COURSES.DESCRIPTION COURSE_DESCRIPTION
FROM COURSES_GROUPS CG
         JOIN VW_GROUPS ON CG.GROUP_ID = VW_GROUPS.GROUP_ID
         JOIN VW_COURSES ON CG.COURSE_ID = VW_COURSES.COURSE_ID;

CREATE OR REPLACE VIEW VW_STUDENTS_GROUPS AS
SELECT SG.STUDENT_ID,
       VW_STUDENTS.NAME  STUDENT_NAME,
       VW_STUDENTS.EMAIL STUDENT_EMAIL,
       SG.GROUP_ID,
       VW_GROUPS.GROUP_NAME
FROM STUDENTS_GROUPS SG
         JOIN VW_STUDENTS ON SG.STUDENT_ID = VW_STUDENTS.STUDENT_ID
         JOIN VW_GROUPS ON SG.GROUP_ID = VW_GROUPS.GROUP_ID;

CREATE OR REPLACE VIEW VW_TIMETABLES AS
SELECT TIMETABLE_ID,
       TIMETABLES.GROUP_ID,
       VW_GROUPS.GROUP_NAME,
       ACTUAL_CAPACITY        GROUP_CAPACITY,
       TIMETABLES.CLASSROOM_ID,
       VW_CLASSROOMS.NAME     CLASSROOM_NAME,
       VW_CLASSROOMS.CAPACITY CLASSROOM_CAPACITY,
       TIMETABLES."BEGIN",
       TIMETABLES."END"
FROM TIMETABLES
         JOIN VW_GROUPS ON TIMETABLES.GROUP_ID = VW_GROUPS.GROUP_ID
         JOIN VW_CLASSROOMS ON TIMETABLES.CLASSROOM_ID = VW_CLASSROOMS.CLASSROOM_ID;

CREATE OR REPLACE VIEW VW_GROUP_MESSAGES AS
SELECT GMSG_ID           MESSAGE_ID,
       GM.GROUP_ID,
       VW_GROUPS.GROUP_NAME,
       GM.USER_ID        AUTHOR_ID,
       VW_USERS.USERNAME AUTHOR_USERNAME,
       CONTENT,
       CREATED
FROM GROUP_MESSAGES GM
         JOIN VW_GROUPS ON GM.GROUP_ID = VW_GROUPS.GROUP_ID
         JOIN VW_USERS ON GM.USER_ID = VW_USERS.USER_ID;

CREATE OR REPLACE VIEW VW_COMMENTS AS
SELECT COMMENT_ID,
       COMMENTS.CONTENT                  COMMENT_CONTENT,
       COMMENTS.CREATED                  COMMENT_CREATED,
       COMMENTS.USER_ID                  COMMENT_AUTHOR_ID,
       VW_USERS.USERNAME                 COMMENT_AUTHOR_USERNAME,
       COMMENTS.MESSAGE_ID,
       VW_GROUP_MESSAGES.CONTENT         MESSAGE_CONTENT,
       VW_GROUP_MESSAGES.CREATED         MESSAGE_CREATED,
       VW_GROUP_MESSAGES.AUTHOR_ID       MESSAGE_AUTHOR_ID,
       VW_GROUP_MESSAGES.AUTHOR_USERNAME MESSAGE_AUTHOR_USERNAME
FROM COMMENTS
         JOIN VW_USERS ON COMMENTS.USER_ID = VW_USERS.USER_ID
         JOIN VW_GROUP_MESSAGES ON COMMENTS.MESSAGE_ID = VW_GROUP_MESSAGES.MESSAGE_ID;

CREATE OR REPLACE VIEW VW_PRIVATE_MESSAGES AS
SELECT PMSG_ID             MESSAGE_ID,
       FROM_USER           FROM_USER_ID,
       USERS_FROM.USERNAME FROM_USER_USERNAME,
       TO_USER             TO_USER_ID,
       USERS_TO.USERNAME   TO_USER_USERNAME,
       CONTENT             MESSAGE_CONTENT,
       CREATED             MESSAGE_CREATED
FROM PRIVATE_MESSAGES
         JOIN VW_USERS USERS_FROM ON FROM_USER = USERS_FROM.USER_ID
         JOIN VW_USERS USERS_TO ON TO_USER = USERS_TO.USER_ID;

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_USER
-- *********************************************************************************************************************
-- *********************************************************************************************************************
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
        RETURN REGEXP_LIKE(p_email, '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$');
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

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_STUDENT
-- *********************************************************************************************************************
-- *********************************************************************************************************************
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

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_FILE
-- *********************************************************************************************************************
-- *********************************************************************************************************************
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
        VALUES (p_user_id, p_file_name, p_file_type, p_file_data)
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

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_TEACHER
-- *********************************************************************************************************************
-- *********************************************************************************************************************
    CREATE OR REPLACE PACKAGE PKG_TEACHER AS
    SUBTYPE T_ID IS TEACHERS.TEACHER_ID%TYPE;

    FUNCTION NEW(p_user_id PKG_USER.T_ID) RETURN T_ID;

    FUNCTION IS_TEACHER(p_user_id PKG_USER.T_ID) RETURN BOOLEAN;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_ID(p_teacher_id T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR;
END;

CREATE OR REPLACE PACKAGE BODY PKG_TEACHER AS
    FUNCTION NEW(p_user_id PKG_USER.T_ID) RETURN T_ID AS
        v_id T_ID;
    BEGIN
        INSERT INTO TEACHERS(USER_ID) VALUES (p_user_id) RETURNING TEACHER_ID INTO v_id;
        COMMIT;
        RETURN v_id;
    END;

    FUNCTION IS_TEACHER(p_user_id PKG_USER.T_ID) RETURN BOOLEAN AS
        v_count NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_count FROM TEACHERS WHERE USER_ID = p_user_id;
        RETURN (v_count = 1);
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_TEACHERS;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_ID(p_teacher_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_TEACHERS WHERE TEACHER_ID = p_teacher_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_TEACHERS WHERE USER_ID = p_user_id;
        RETURN v_cursor;
    END;
END;

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_CLASSROOM
-- *********************************************************************************************************************
-- *********************************************************************************************************************
    CREATE OR REPLACE PACKAGE PKG_CLASSROOM AS
    SUBTYPE T_ID IS CLASSROOMS.CLASSROOM_ID%TYPE;
    SUBTYPE T_NAME IS CLASSROOMS.NAME%TYPE;
    SUBTYPE T_CAPACITY IS CLASSROOMS.CAPACITY%TYPE;

    FUNCTION NEW(p_name T_NAME, p_capacity T_CAPACITY) RETURN T_ID;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_ID(p_classroom_id T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_CAPACITY(p_capacity T_CAPACITY) RETURN SYS_REFCURSOR;

    PROCEDURE UPDATE_NAME(p_classroom_id T_ID, p_name T_NAME);

    PROCEDURE UPDATE_CAPACITY(p_classroom_id T_ID, p_capacity T_CAPACITY);

    PROCEDURE REMOVE(p_classroom_id T_ID);
END;

CREATE OR REPLACE PACKAGE BODY PKG_CLASSROOM AS
    FUNCTION NEW(p_name T_NAME, p_capacity T_CAPACITY) RETURN T_ID AS
        v_id T_ID;
    BEGIN
        INSERT INTO CLASSROOMS(NAME, CAPACITY) VALUES (p_name, p_capacity) RETURNING CLASSROOM_ID INTO v_id;
        COMMIT;
        RETURN v_id;
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_CLASSROOMS;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_ID(p_classroom_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_CLASSROOMS WHERE CLASSROOM_ID = p_classroom_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_CAPACITY(p_capacity T_CAPACITY) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_CLASSROOMS WHERE CAPACITY >= p_capacity ORDER BY CAPACITY;
        RETURN v_cursor;
    END;

    PROCEDURE UPDATE_NAME(p_classroom_id T_ID, p_name T_NAME) AS
    BEGIN
        UPDATE CLASSROOMS SET NAME = p_name WHERE CLASSROOM_ID = p_classroom_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_CAPACITY(p_classroom_id T_ID, p_capacity T_CAPACITY) AS
    BEGIN
        UPDATE CLASSROOMS SET CAPACITY = p_capacity WHERE CLASSROOM_ID = p_classroom_id;
        COMMIT;
    END;

    PROCEDURE REMOVE(p_classroom_id T_ID) AS
    BEGIN
        DELETE FROM CLASSROOMS WHERE CLASSROOM_ID = p_classroom_id;
    END;
END;

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_COURSE
-- *********************************************************************************************************************
-- *********************************************************************************************************************
    CREATE OR REPLACE PACKAGE PKG_COURSE AS
    SUBTYPE T_ID IS COURSES.COURSE_ID%TYPE;
    SUBTYPE T_FULL_NAME IS COURSES.FULL_NAME%TYPE;
    SUBTYPE T_SHORT_NAME IS COURSES.SHORT_NAME%TYPE;
    SUBTYPE T_DESCRIPTION IS COURSES.DESCRIPTION%TYPE;

    FUNCTION NEW(p_full_name T_FULL_NAME, p_short_name T_SHORT_NAME, p_description T_DESCRIPTION) RETURN T_ID;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_ID(p_course_id T_ID) RETURN SYS_REFCURSOR;

    PROCEDURE REMOVE(p_course_id T_ID);

    PROCEDURE UPDATE_FULL_NAME(p_course_id T_ID, p_full_name T_FULL_NAME);

    PROCEDURE UPDATE_SHORT_NAME(p_course_id T_ID, p_short_name T_SHORT_NAME);

    PROCEDURE UPDATE_DESCRIPTION(p_course_id T_ID, p_description T_DESCRIPTION);
END;

CREATE OR REPLACE PACKAGE BODY PKG_COURSE AS

    FUNCTION NEW(p_full_name T_FULL_NAME, p_short_name T_SHORT_NAME, p_description T_DESCRIPTION) RETURN T_ID AS
        v_id T_ID;
    BEGIN
        INSERT INTO COURSES(FULL_NAME, SHORT_NAME, DESCRIPTION)
        VALUES (p_full_name, p_short_name, p_description)
        RETURNING COURSE_ID INTO v_id;
        COMMIT;
        RETURN v_id;
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_COURSES;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_ID(p_course_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_COURSES WHERE COURSE_ID = p_course_id;
        RETURN v_cursor;
    END;

    PROCEDURE REMOVE(p_course_id T_ID) AS
    BEGIN
        DELETE FROM COURSES WHERE COURSE_ID = p_course_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_FULL_NAME(p_course_id T_ID, p_full_name T_FULL_NAME) AS
    BEGIN
        UPDATE COURSES SET FULL_NAME = p_full_name WHERE COURSE_ID = p_course_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_SHORT_NAME(p_course_id T_ID, p_short_name T_SHORT_NAME) AS
    BEGIN
        UPDATE COURSES SET SHORT_NAME = p_short_name WHERE COURSE_ID = p_course_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_DESCRIPTION(p_course_id T_ID, p_description T_DESCRIPTION) AS
    BEGIN
        UPDATE COURSES SET DESCRIPTION = p_description WHERE COURSE_ID = p_course_id;
        COMMIT;
    END;
END;

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_GRADE
-- *********************************************************************************************************************
-- *********************************************************************************************************************
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

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_USERS_STATUS
-- *********************************************************************************************************************
-- *********************************************************************************************************************
    CREATE OR REPLACE PACKAGE PKG_USERS_STATUS AS
    SUBTYPE T_ID IS USERS_STATUS.STATUS_ID%TYPE;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_ID(p_status_id T_ID) RETURN SYS_REFCURSOR;
END;

CREATE OR REPLACE PACKAGE BODY PKG_USERS_STATUS AS
    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_USERS_STATUS;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_ID(p_status_id T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_USERS_STATUS WHERE STATUS_ID = p_status_id;
        RETURN v_cursor;
    END;
END;

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_GROUPS
-- *********************************************************************************************************************
-- *********************************************************************************************************************
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
        v_count           NUMBER;
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

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_GROUPS
-- *********************************************************************************************************************
-- *********************************************************************************************************************
    CREATE OR REPLACE PACKAGE PKG_TIMETABLE AS
    SUBTYPE T_ID IS TIMETABLES.TIMETABLE_ID%TYPE;
    SUBTYPE T_BEGIN IS TIMETABLES."BEGIN"%TYPE;
    SUBTYPE T_END IS TIMETABLEs."END"%TYPE;

    FUNCTION NEW(p_group_id PKG_GROUP.T_ID, p_classroom_id PKG_CLASSROOM.T_ID, p_begin T_BEGIN, p_end T_END) RETURN T_ID;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_GROUP(p_group_id PKG_GROUP.T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_CLASSROOM(p_classroom_id PKG_CLASSROOM.T_ID) RETURN SYS_REFCURSOR;

    PROCEDURE REMOVE(p_timetable_id T_ID);

    PROCEDURE UPDATE_BEGIN(p_timetable_id T_ID, p_begin T_BEGIN);

    PROCEDURE UPDATE_END(p_timetable_id T_ID, p_end T_END);
END;

CREATE OR REPLACE PACKAGE BODY PKG_TIMETABLE AS
    FUNCTION VALID_BEGIN_END(p_begin T_BEGIN, p_end T_END) RETURN BOOLEAN AS
    BEGIN
        RETURN ((p_end - p_begin) >= 0);
    END;

    FUNCTION NEW(p_group_id PKG_GROUP.T_ID, p_classroom_id PKG_CLASSROOM.T_ID, p_begin T_BEGIN,
                 p_end T_END) RETURN T_ID AS
        v_id                 T_ID;
        v_classroom_capacity PKG_CLASSROOM.T_CAPACITY;
        v_group_capacity     PKG_GROUP.T_ACTUAL_CAPACITY;
    BEGIN
        IF NOT VALID_BEGIN_END(p_begin, p_end) THEN
            RAISE_APPLICATION_ERROR(-20001, 'Cannot create new timetable! Begin and end of the session are not valid!');
        end if;

        SELECT CAPACITY INTO v_classroom_capacity FROM VW_CLASSROOMS;
        SELECT ACTUAL_CAPACITY INTO v_group_capacity FROM VW_GROUPS;

        IF (v_classroom_capacity < v_group_capacity) THEN
            RAISE_APPLICATION_ERROR(-20001, 'Cannot create new timetable! Capacity of the classroom is too small!');
        END IF;

        INSERT INTO TIMETABLES(GROUP_ID, CLASSROOM_ID, "BEGIN", "END")
        VALUES (p_group_id, p_classroom_id, p_begin, p_end)
        RETURNING TIMETABLE_ID INTO v_id;
        COMMIT;
        RETURN v_id;
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_TIMETABLES;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_GROUP(p_group_id PKG_GROUP.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_TIMETABLES WHERE GROUP_ID = p_group_id;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_CLASSROOM(p_classroom_id PKG_CLASSROOM.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_TIMETABLES WHERE CLASSROOM_ID = p_classroom_id;
        RETURN v_cursor;
    END;

    PROCEDURE REMOVE(p_timetable_id T_ID) AS
    BEGIN
        DELETE TIMETABLES WHERE TIMETABLE_ID = p_timetable_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_BEGIN(p_timetable_id T_ID, p_begin T_BEGIN) AS
    BEGIN
        UPDATE TIMETABLES SET "BEGIN" = p_begin WHERE TIMETABLE_ID = p_timetable_id;
        COMMIT;
    END;

    PROCEDURE UPDATE_END(p_timetable_id T_ID, p_end T_END) AS
    BEGIN
        UPDATE TIMETABLES SET "END" = p_end WHERE TIMETABLE_ID = p_timetable_id;
        COMMIT;
    END;
END;

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_PMSG
-- *********************************************************************************************************************
-- *********************************************************************************************************************
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

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_GMSG
-- *********************************************************************************************************************
-- *********************************************************************************************************************
    CREATE OR REPLACE PACKAGE PKG_GMSG AS
    SUBTYPE T_ID IS GROUP_MESSAGES.GMSG_ID%TYPE;
    SUBTYPE T_CONTENT IS GROUP_MESSAGES.CONTENT%TYPE;
    SUBTYPE T_CREATED IS GROUP_MESSAGES.CREATED%TYPE;

    FUNCTION NEW(p_group PKG_GROUP.T_ID, p_user PKG_USER.T_ID, p_content T_CONTENT) RETURN T_ID;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_ID(p_message T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_GROUP(p_group PKG_GROUP.T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_USER(p_user PKG_USER.T_ID) RETURN SYS_REFCURSOR;

    PROCEDURE UPDATE_GROUP(p_message T_ID, p_group PKG_GROUP.T_ID);

    PROCEDURE UPDATE_USER(p_message T_ID, p_user PKG_USER.T_ID);

    PROCEDURE UPDATE_CONTENT(p_message T_ID, p_content T_CONTENT);

    PROCEDURE REMOVE(p_message T_ID);
END;

CREATE OR REPLACE PACKAGE BODY PKG_GMSG AS
    FUNCTION NEW(p_group PKG_GROUP.T_ID, p_user PKG_USER.T_ID, p_content T_CONTENT) RETURN T_ID AS
        v_id T_ID;
    BEGIN
        INSERT INTO GROUP_MESSAGES(GROUP_ID, USER_ID, CONTENT)
        VALUES (p_group, p_user, p_content)
        RETURNING GMSG_ID INTO v_id;
        COMMIT;
        RETURN v_id;
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GROUP_MESSAGES;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_ID(p_message T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GROUP_MESSAGES WHERE MESSAGE_ID = p_message;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_GROUP(p_group PKG_GROUP.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GROUP_MESSAGES WHERE GROUP_ID = p_group;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_USER(p_user PKG_USER.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_GROUP_MESSAGES WHERE AUTHOR_ID = p_user;
        RETURN v_cursor;
    END;

    PROCEDURE UPDATE_GROUP(p_message T_ID, p_group PKG_GROUP.T_ID) AS
    BEGIN
        UPDATE GROUP_MESSAGES SET GROUP_ID = p_group WHERE GMSG_ID = p_message;
        COMMIT;
    END;

    PROCEDURE UPDATE_USER(p_message T_ID, p_user PKG_USER.T_ID) AS
    BEGIN
        UPDATE GROUP_MESSAGES SET USER_ID = p_user WHERE GMSG_ID = p_message;
        COMMIT;
    END;

    PROCEDURE UPDATE_CONTENT(p_message T_ID, p_content T_CONTENT) AS
    BEGIN
        UPDATE GROUP_MESSAGES SET CONTENT = p_content WHERE GMSG_ID = p_message;
        COMMIT;
    END;

    PROCEDURE REMOVE(p_message T_ID) AS
    BEGIN
        DELETE FROM GROUP_MESSAGES WHERE GMSG_ID = p_message;
        COMMIT;
    END;
END;

-- *********************************************************************************************************************
-- *********************************************************************************************************************
-- BALÍČKY --- PKG_COMMENT
-- *********************************************************************************************************************
-- *********************************************************************************************************************
    CREATE OR REPLACE PACKAGE PKG_COMMENT AS
    SUBTYPE T_ID IS COMMENTS.COMMENT_ID%TYPE;
    SUBTYPE T_CONTENT IS COMMENTS.CONTENT%TYPE;
    SUBTYPE T_CREATED IS COMMENTS.CREATED%TYPE;

    FUNCTION NEW(p_user PKG_USER.T_ID, p_message PKG_GMSG.T_ID, p_content T_CONTENT) RETURN T_ID;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_ID(p_comment T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_USER(p_user PKG_USER.T_ID) RETURN SYS_REFCURSOR;

    FUNCTION GET_BY_MESSAGE(p_message PKG_GMSG.T_ID) RETURN SYS_REFCURSOR;

    PROCEDURE UPDATE_USER(p_comment T_ID, p_user PKG_USER.T_ID);

    PROCEDURE UPDATE_MESSAGE(p_comment T_ID, p_message PKG_GMSG.T_ID);

    PROCEDURE UPDATE_CONTENT(p_comment T_ID, p_content T_CONTENT);

    PROCEDURE REMOVE(p_comment T_ID);
END;

CREATE OR REPLACE PACKAGE BODY PKG_COMMENT AS
    FUNCTION NEW(p_user PKG_USER.T_ID, p_message PKG_GMSG.T_ID, p_content T_CONTENT) RETURN T_ID AS
        v_id T_ID;
    BEGIN
        INSERT INTO COMMENTS(USER_ID, MESSAGE_ID, CONTENT)
        VALUES (p_user, p_message, p_content)
        RETURNING COMMENT_ID INTO v_id;
        COMMIT;
        RETURN v_id;
    END;

    FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_COMMENTS;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_ID(p_comment T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_COMMENTS WHERE COMMENT_ID = p_comment;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_USER(p_user PKG_USER.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_COMMENTS WHERE COMMENT_AUTHOR_ID = p_user;
        RETURN v_cursor;
    END;

    FUNCTION GET_BY_MESSAGE(p_message PKG_GMSG.T_ID) RETURN SYS_REFCURSOR AS
        v_cursor SYS_REFCURSOR;
    BEGIN
        OPEN v_cursor FOR SELECT * FROM VW_COMMENTS WHERE MESSAGE_ID = p_message;
        RETURN v_cursor;
    END;

    PROCEDURE UPDATE_USER(p_comment T_ID, p_user PKG_USER.T_ID) AS
    BEGIN
        UPDATE COMMENTS SET USER_ID = p_user WHERE COMMENT_ID = p_comment;
        COMMIT;
    END;

    PROCEDURE UPDATE_MESSAGE(p_comment T_ID, p_message PKG_GMSG.T_ID) AS
    BEGIN
        UPDATE COMMENTS SET MESSAGE_ID = p_message WHERE COMMENT_ID = p_comment;
        COMMIT;
    END;

    PROCEDURE UPDATE_CONTENT(p_comment T_ID, p_content T_CONTENT) AS
    BEGIN
        UPDATE COMMENTS SET CONTENT = p_content WHERE COMMENT_ID = p_comment;
        COMMIT;
    END;

    PROCEDURE REMOVE(p_comment T_ID) AS
    BEGIN
        DELETE FROM COMMENTS WHERE COMMENT_ID = p_comment;
        COMMIT;
    END;
END;