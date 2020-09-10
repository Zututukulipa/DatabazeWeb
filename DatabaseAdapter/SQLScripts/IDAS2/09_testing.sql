-- ||| Vložení nového uživatele
DECLARE
    v_var USERS.USER_ID%TYPE;
BEGIN
    v_var := USER_NEW('admin', 'heslo', 'Jan', '', 'Bedna', 'admin@upce.cz', 1);
    DBMS_OUTPUT.PUT_LINE('NEW USER: ' || v_var);
end;

-- ||| Nastavení uživateli administrátorská práva
BEGIN
    USER_UPDATE_ADMIN(USER_LOGIN('admin', 'heslo'), 1);
end;

-- ||| Přihlášení uživatele do systému
DECLARE
    v_var USERS.USER_ID%TYPE;
BEGIN
    v_var := USER_LOGIN('hejduk', 'qwertz');
    DBMS_OUTPUT.PUT_LINE('LOGIN USER: ' || v_var);
end;

DECLARE
    v_var USERS.USER_ID%TYPE;
BEGIN
    v_var := USER_LOGIN('nothing', 'nothing');
    DBMS_OUTPUT.PUT_LINE('LOGIN USER: ' || v_var);
end;

-- ||| Získání cursoru s daty o uživateli
DECLARE
    v_cursor SYS_REFCURSOR := USER_GET(1);
    v_row VW_USERS%ROWTYPE;
BEGIN
    LOOP
        FETCH v_cursor INTO v_row;
        EXIT WHEN v_cursor%NOTFOUND;
        DBMS_OUTPUT.PUT_LINE(v_row.LAST_NAME);
    end loop;
end;

-- ||| Změna stavu uživatele
BEGIN
    USER_UPDATE_STATUS(2, 1);
end;

-- ||| Změna přihlašovacích údajů uživatele
BEGIN
    USER_UPDATE_LOGIN(2, 'hejduk', 'qwertz');
end;

-- ||| Změna základních údajů o uživateli
BEGIN
    USER_UPDATE_DETAILS(2, 'Jiří', '', 'Hejduk', 'st11111@student.upce.cz');
end;

-- ||| Vytvoření nového studenta
DECLARE
    v_var STUDENTS.STUDENT_ID%TYPE;
BEGIN
    v_var := STUDENT_NEW(2);
end;

-- ||| Změna ročníku studenta
BEGIN
    STUDENT_UPDATE_YEAR(1, 3);
end;