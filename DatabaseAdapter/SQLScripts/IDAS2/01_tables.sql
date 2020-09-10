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

