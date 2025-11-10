SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

CREATE FUNCTION public.add_achievement(p_student_id integer, p_event_name text, p_event_date timestamp without time zone, p_level text, p_place integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE new_id integer;
BEGIN INSERT INTO achievement (student_id, event_name, event_date, level, place) VALUES (p_student_id, p_event_name, p_event_date, p_level, p_place) RETURNING id INTO new_id; RETURN new_id; END;
$$;

ALTER FUNCTION public.add_achievement(p_student_id integer, p_event_name text, p_event_date timestamp without time zone, p_level text, p_place integer) OWNER TO postgres;

CREATE FUNCTION public.add_bulk_grades(p_student_ids integer[], p_lesson_id integer, p_grade integer, p_work_type text, p_date date) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM gradebook gb
    WHERE gb.lesson_id = p_lesson_id AND gb.student_id = ANY(p_student_ids);
    IF p_grade = 0 THEN
        RETURN;
    END IF;
    IF p_grade IS NOT NULL OR p_work_type = 'Н' THEN
        INSERT INTO gradebook(student_id, lesson_id, grade, grade_date, work_type, grade_time)
        SELECT
            student_id,
            p_lesson_id,
            p_grade,
            p_date,
            p_work_type,
            NOW()::time
        FROM UNNEST(p_student_ids) AS t(student_id);
    END IF;
    UPDATE lesson SET lesson_date = p_date
    WHERE id = p_lesson_id AND lesson_date IS NULL;
END;
$$;

ALTER FUNCTION public.add_bulk_grades(p_student_ids integer[], p_lesson_id integer, p_grade integer, p_work_type text, p_date date) OWNER TO postgres;

CREATE FUNCTION public.add_class(p_letter text, p_parallel_number integer, p_head_teacher_id integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE v_parallel_id integer; new_class_id integer;
BEGIN
    SELECT id INTO v_parallel_id FROM parallel WHERE "number" = p_parallel_number;
    IF v_parallel_id IS NULL THEN INSERT INTO parallel ("number") VALUES (p_parallel_number) RETURNING id INTO v_parallel_id; END IF;
    INSERT INTO class (letter, parallel_id, head_teacher_id) VALUES (substring(p_letter from 1 for 1), v_parallel_id, p_head_teacher_id) RETURNING id INTO new_class_id;
    RETURN new_class_id;
END;
$$;

ALTER FUNCTION public.add_class(p_letter text, p_parallel_number integer, p_head_teacher_id integer) OWNER TO postgres;

CREATE FUNCTION public.add_grade_to_lesson(p_student_id integer, p_lesson_id integer, p_discipline_id integer, p_grade integer, p_date date, p_work_type text) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_real_lesson_id INT := p_lesson_id;
    v_lesson_number INT;
    v_workload_id INT;
    new_grade_id INT := -1;
BEGIN
    IF p_lesson_id <= 0 THEN
        v_lesson_number := p_lesson_id * -1;
        v_workload_id := find_or_create_workload_for_primary(p_student_id, p_discipline_id);
        SELECT id INTO v_real_lesson_id FROM lesson WHERE workload_id = v_workload_id AND lesson_number = v_lesson_number;
        IF v_real_lesson_id IS NULL THEN
            INSERT INTO lesson (workload_id, lesson_number) VALUES (v_workload_id, v_lesson_number) RETURNING id INTO v_real_lesson_id;
        END IF;
    END IF;

    IF p_grade IS NOT NULL THEN
        INSERT INTO gradebook(student_id, lesson_id, grade, grade_date, work_type, grade_time)
        VALUES (p_student_id, v_real_lesson_id, p_grade, p_date, p_work_type, NOW()::time)
        RETURNING id INTO new_grade_id;
    END IF;

    UPDATE lesson SET lesson_date = p_date WHERE id = v_real_lesson_id AND (lesson_date IS NULL OR lesson_date != p_date);

    RETURN new_grade_id;
END;
$$;

ALTER FUNCTION public.add_grade_to_lesson(p_student_id integer, p_lesson_id integer, p_discipline_id integer, p_grade integer, p_date date, p_work_type text) OWNER TO postgres;

CREATE FUNCTION public.add_parent(p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_phone character varying, p_email character varying, p_login character varying, p_password_hash bytea, p_password_salt bytea) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE new_parent_id integer;
BEGIN
    INSERT INTO parent(last_name, first_name, patronymic, phone, email, login, password_hash, password_salt)
    VALUES (p_last_name, p_first_name, p_patronymic, p_phone, p_email, p_login, p_password_hash, p_password_salt)
    RETURNING id INTO new_parent_id;
    RETURN new_parent_id;
END;
$$;

ALTER FUNCTION public.add_parent(p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_phone character varying, p_email character varying, p_login character varying, p_password_hash bytea, p_password_salt bytea) OWNER TO postgres;

CREATE FUNCTION public.add_student(p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_class_id integer, p_birth_date date, p_notes text) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
    new_student_id INT;
BEGIN
    INSERT INTO student(last_name, first_name, patronymic, class_id, birth_date, notes, 
                        enrollment_year, gender, status, enrollment_date, status_change_date)
    VALUES (p_last_name, p_first_name, p_patronymic, p_class_id, p_birth_date, p_notes,
            EXTRACT(YEAR FROM current_date), 'М', 'Active', CURRENT_DATE, CURRENT_DATE)
    RETURNING id INTO new_student_id;
    RETURN new_student_id;
END;
$$;

ALTER FUNCTION public.add_student(p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_class_id integer, p_birth_date date, p_notes text) OWNER TO postgres;

CREATE FUNCTION public.add_study_plan(p_name text, p_academic_year_id integer, p_parallel_number integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE v_parallel_id integer; new_plan_id integer;
BEGIN
    SELECT id INTO v_parallel_id FROM parallel WHERE "number" = p_parallel_number;
    IF v_parallel_id IS NULL THEN
        INSERT INTO parallel ("number") VALUES (p_parallel_number) RETURNING id INTO v_parallel_id;
    END IF;
    INSERT INTO study_plan (name, academic_year_id, parallel_id) VALUES (p_name, p_academic_year_id, v_parallel_id) RETURNING id INTO new_plan_id;
    RETURN new_plan_id;
END; $$;

ALTER FUNCTION public.add_study_plan(p_name text, p_academic_year_id integer, p_parallel_number integer) OWNER TO postgres;

CREATE FUNCTION public.add_study_plan(p_name text, p_academic_year text, p_parallel_number integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_parallel_id integer;
    new_plan_id integer;
BEGIN
    SELECT id INTO v_parallel_id FROM parallel WHERE "number" = p_parallel_number;
    IF v_parallel_id IS NULL THEN
        INSERT INTO parallel ("number") VALUES (p_parallel_number) RETURNING id INTO v_parallel_id;
    END IF;
    INSERT INTO study_plan (name, academic_year, parallel_id)
    VALUES (p_name, p_academic_year, v_parallel_id)
    RETURNING id INTO new_plan_id;
    
    RETURN new_plan_id;
END;
$$;

ALTER FUNCTION public.add_study_plan(p_name text, p_academic_year text, p_parallel_number integer) OWNER TO postgres;

CREATE FUNCTION public.add_teacher(p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_phone character varying, p_email character varying, p_login character varying, p_password_hash bytea, p_password_salt bytea, p_role character varying, p_notes text) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE new_id integer;
BEGIN
    INSERT INTO teacher (last_name, first_name, patronymic, phone, email, login, password_hash, password_salt, role, notes)
    VALUES (p_last_name, p_first_name, p_patronymic, p_phone, p_email, p_login, p_password_hash, p_password_salt, p_role, p_notes)
    RETURNING id INTO new_id;
    RETURN new_id;
END;
$$;

ALTER FUNCTION public.add_teacher(p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_phone character varying, p_email character varying, p_login character varying, p_password_hash bytea, p_password_salt bytea, p_role character varying, p_notes text) OWNER TO postgres;

CREATE FUNCTION public.change_user_password(p_user_id integer, p_user_role character varying, p_new_hash bytea, p_new_salt bytea) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF p_user_role = 'admin' OR p_user_role = 'teacher' THEN
        UPDATE teacher
        SET password_hash = p_new_hash,
            password_salt = p_new_salt
        WHERE id = p_user_id;
    ELSIF p_user_role = 'student' THEN
        UPDATE student
        SET password_hash = p_new_hash,
            password_salt = p_new_salt
        WHERE id = p_user_id;
    ELSIF p_user_role = 'parent' THEN
        UPDATE parent
        SET password_hash = p_new_hash,
            password_salt = p_new_salt
        WHERE id = p_user_id;
    END IF;
END;
$$;

ALTER FUNCTION public.change_user_password(p_user_id integer, p_user_role character varying, p_new_hash bytea, p_new_salt bytea) OWNER TO postgres;

CREATE FUNCTION public.check_grade_value() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.grade < 1 OR NEW.grade > 5 THEN
        RAISE EXCEPTION 'Некорректная оценка: %. Оценка должна быть в диапазоне от 1 до 5.', NEW.grade;
    END IF;
    RETURN NEW;
END;
$$;

ALTER FUNCTION public.check_grade_value() OWNER TO postgres;

CREATE FUNCTION public.delete_achievement(p_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN DELETE FROM achievement WHERE id = p_id; END;
$$;

ALTER FUNCTION public.delete_achievement(p_id integer) OWNER TO postgres;

CREATE FUNCTION public.delete_grade(p_gradebook_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN DELETE FROM gradebook WHERE id = p_gradebook_id; END;
$$;

ALTER FUNCTION public.delete_grade(p_gradebook_id integer) OWNER TO postgres;

CREATE FUNCTION public.delete_study_plan_item(p_item_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN DELETE FROM study_plan_item WHERE id = p_item_id; END;
$$;

ALTER FUNCTION public.delete_study_plan_item(p_item_id integer) OWNER TO postgres;

CREATE FUNCTION public.delete_teacher(p_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_role_to_delete VARCHAR;
    v_admin_count INT;
BEGIN
    SELECT role INTO v_role_to_delete FROM teacher WHERE id = p_id;

    IF v_role_to_delete = 'admin' THEN
        SELECT count(*) INTO v_admin_count FROM teacher WHERE role = 'admin';
        IF v_admin_count <= 1 THEN
            RAISE EXCEPTION 'Невозможно удалить последнего администратора. В системе должен оставаться хотя бы один администратор.';
        END IF;
    END IF;
    UPDATE class SET head_teacher_id = NULL WHERE head_teacher_id = p_id;
    DELETE FROM teacher WHERE id = p_id;
END;
$$;

ALTER FUNCTION public.delete_teacher(p_id integer) OWNER TO postgres;

CREATE FUNCTION public.delete_workload(p_class_id integer, p_discipline_id integer, p_academic_year_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN DELETE FROM workload WHERE class_id = p_class_id AND discipline_id = p_discipline_id AND academic_year_id = p_academic_year_id; END;
$$;

ALTER FUNCTION public.delete_workload(p_class_id integer, p_discipline_id integer, p_academic_year_id integer) OWNER TO postgres;

CREATE FUNCTION public.delete_workload(p_class_id integer, p_discipline_id integer, p_academic_year character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN DELETE FROM workload WHERE class_id = p_class_id AND discipline_id = p_discipline_id AND academic_year = p_academic_year; END;
$$;

ALTER FUNCTION public.delete_workload(p_class_id integer, p_discipline_id integer, p_academic_year character varying) OWNER TO postgres;

CREATE FUNCTION public.expel_student(p_student_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE student SET status = 'Expelled', status_change_date = CURRENT_DATE WHERE id = p_student_id;
END;
$$;

ALTER FUNCTION public.expel_student(p_student_id integer) OWNER TO postgres;

CREATE FUNCTION public.find_or_create_workload_for_primary(p_student_id integer, p_discipline_id integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_workload_id INT;
    v_class_id INT;
    v_head_teacher_id INT;
    v_latest_academic_year VARCHAR(9);
BEGIN
    SELECT s.class_id, c.head_teacher_id INTO v_class_id, v_head_teacher_id
    FROM student s JOIN class c ON s.class_id = c.id WHERE s.id = p_student_id;

    SELECT MAX(sp.academic_year) INTO v_latest_academic_year
    FROM study_plan sp JOIN class c ON c.id = v_class_id AND sp.parallel_id = c.parallel_id;

    SELECT id INTO v_workload_id FROM workload WHERE class_id = v_class_id AND discipline_id = p_discipline_id AND academic_year = v_latest_academic_year;

    IF v_workload_id IS NULL THEN
        IF v_head_teacher_id IS NULL THEN
             RAISE EXCEPTION 'Невозможно создать нагрузку: у этого начального класса не назначен классный руководитель.';
        END IF;

        INSERT INTO workload (class_id, discipline_id, teacher_id, academic_year)
        VALUES (v_class_id, p_discipline_id, v_head_teacher_id, v_latest_academic_year)
        RETURNING id INTO v_workload_id;
    END IF;

    RETURN v_workload_id;
END;
$$;

ALTER FUNCTION public.find_or_create_workload_for_primary(p_student_id integer, p_discipline_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_academic_performance_summary(p_academic_year_id integer, p_start_date date, p_end_date date) RETURNS TABLE(parallel_number smallint, class_name text, discipline_name character varying, avg_grade numeric, quality_percent numeric, success_percent numeric, total_grades bigint)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    WITH
    class_discipline_map AS (
        SELECT
            c.id as class_id,
            d.id as discipline_id,
            p.number as p_number,
            (p.number || ' "' || c.letter || '"')::text as c_name,
            d.name as d_name
        FROM class c
        JOIN parallel p ON c.parallel_id = p.id
        JOIN study_plan sp ON sp.parallel_id = p.id
        JOIN study_plan_item spi ON spi.study_plan_id = sp.id
        JOIN discipline d ON spi.discipline_id = d.id
        WHERE sp.academic_year_id = p_academic_year_id
    ),
    grades_in_period AS (
        SELECT
            w.class_id,
            w.discipline_id,
            gb.grade
        FROM gradebook gb
        JOIN lesson l ON gb.lesson_id = l.id
        JOIN workload w ON l.workload_id = w.id
        WHERE w.academic_year_id = p_academic_year_id
          AND gb.grade_date BETWEEN p_start_date AND p_end_date
    )
    SELECT
        cdm.p_number,
        cdm.c_name,
        cdm.d_name,
        COALESCE(ROUND(AVG(gip.grade), 2), 0) as avg_grade,
        COALESCE(ROUND(100.0 * COUNT(gip.grade) FILTER (WHERE gip.grade >= 4) / NULLIF(COUNT(gip.grade), 0), 2), 0) as quality_percent,
        COALESCE(ROUND(100.0 * COUNT(gip.grade) FILTER (WHERE gip.grade >= 3) / NULLIF(COUNT(gip.grade), 0), 2), 0) as success_percent,
        COUNT(gip.grade) as total_grades
    FROM class_discipline_map cdm
    LEFT JOIN grades_in_period gip ON cdm.class_id = gip.class_id AND cdm.discipline_id = gip.discipline_id
    GROUP BY cdm.p_number, cdm.c_name, cdm.d_name
    ORDER BY cdm.p_number, cdm.c_name, cdm.d_name;
END;
$$;

ALTER FUNCTION public.get_academic_performance_summary(p_academic_year_id integer, p_start_date date, p_end_date date) OWNER TO postgres;

CREATE FUNCTION public.get_all_classes_for_teacher(p_teacher_id integer) RETURNS TABLE(id integer, name text)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT 
        c.id, 
        (p.number || ' "' || c.letter || '"')::text AS "name"
    FROM workload w 
    JOIN class c ON w.class_id = c.id 
    JOIN parallel p ON c.parallel_id = p.id
    WHERE w.teacher_id = p_teacher_id

    UNION
    SELECT 
        c.id, 
        (p.number || ' "' || c.letter || '"')::text AS "name"
    FROM class c
    JOIN parallel p ON c.parallel_id = p.id
    WHERE c.head_teacher_id = p_teacher_id
    
    ORDER BY "name";
END;
$$;

ALTER FUNCTION public.get_all_classes_for_teacher(p_teacher_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_all_classes_with_details() RETURNS TABLE(class_id integer, class_name text, parallel_number smallint, head_teacher_id integer, head_teacher_full_name text)
    LANGUAGE plpgsql
    AS $$
BEGIN RETURN QUERY SELECT c.id, (p.number || ' "' || c.letter || '"')::text, p.number, c.head_teacher_id, concat_ws(' ', t.last_name, t.first_name, t.patronymic)::text FROM class c JOIN parallel p ON c.parallel_id = p.id LEFT JOIN teacher t ON c.head_teacher_id = t.id ORDER BY p.number, c.letter; END;
$$;

ALTER FUNCTION public.get_all_classes_with_details() OWNER TO postgres;

CREATE FUNCTION public.get_all_disciplines() RETURNS TABLE(id integer, name character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN RETURN QUERY SELECT d.id, d.name FROM discipline d ORDER BY d.name; END;
$$;

ALTER FUNCTION public.get_all_disciplines() OWNER TO postgres;

CREATE FUNCTION public.get_all_parents_for_linking(p_student_id integer) RETURNS TABLE(id integer, last_name character varying, first_name character varying, patronymic character varying, phone character varying, login character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT p.id, p.last_name, p.first_name, p.patronymic, p.phone, p.login
    FROM parent p
    WHERE NOT EXISTS (
        SELECT 1 FROM student_parent sp
        WHERE sp.parent_id = p.id AND sp.student_id = p_student_id
    )
    ORDER BY p.last_name, p.first_name;
END;
$$;

ALTER FUNCTION public.get_all_parents_for_linking(p_student_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_all_study_plans() RETURNS TABLE(id integer, name character varying, academic_year_id integer, academic_year_name character varying, parallel_id integer, parallel_number smallint)
    LANGUAGE plpgsql
    AS $$
BEGIN 
    RETURN QUERY SELECT sp.id, sp.name, sp.academic_year_id, ay.name as academic_year_name, sp.parallel_id, p.number 
    FROM study_plan sp 
    JOIN parallel p ON sp.parallel_id = p.id
    JOIN academic_year ay ON sp.academic_year_id = ay.id
    ORDER BY ay.start_date DESC, p.number; 
END; $$;

ALTER FUNCTION public.get_all_study_plans() OWNER TO postgres;

CREATE FUNCTION public.get_all_teachers() RETURNS TABLE(id integer, last_name character varying, first_name character varying, patronymic character varying, phone character varying, email character varying, login character varying, role character varying, notes text)
    LANGUAGE plpgsql
    AS $$
BEGIN RETURN QUERY SELECT t.id, t.last_name, t.first_name, t.patronymic, t.phone, t.email, t.login, t.role, t.notes FROM teacher t ORDER BY t.last_name, t.first_name; END;
$$;

ALTER FUNCTION public.get_all_teachers() OWNER TO postgres;

CREATE FUNCTION public.get_disciplines_for_student(p_student_id integer, p_teacher_id integer DEFAULT NULL::integer) RETURNS TABLE(discipline_id integer, discipline_name character varying)
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_class_id INT;
    v_is_primary BOOLEAN;
    v_head_teacher_id INT;
    v_latest_academic_year_id INT;
BEGIN
    SELECT s.class_id, c.is_primary_school, c.head_teacher_id
    INTO v_class_id, v_is_primary, v_head_teacher_id
    FROM student s JOIN class c ON s.class_id = c.id
    WHERE s.id = p_student_id;
    SELECT sp.academic_year_id INTO v_latest_academic_year_id
    FROM study_plan sp
    JOIN academic_year ay ON sp.academic_year_id = ay.id
    WHERE sp.parallel_id = (SELECT parallel_id FROM class WHERE id = v_class_id)
    ORDER BY ay.start_date DESC
    LIMIT 1;

    IF v_latest_academic_year_id IS NULL THEN RETURN; END IF;

    IF p_teacher_id IS NULL THEN
        RETURN QUERY
        SELECT DISTINCT d.id, d.name
        FROM study_plan_item spi
        JOIN study_plan sp ON spi.study_plan_id = sp.id
        JOIN discipline d ON spi.discipline_id = d.id
        WHERE sp.parallel_id = (SELECT parallel_id FROM class WHERE id = v_class_id)
          AND sp.academic_year_id = v_latest_academic_year_id
        ORDER BY d.name;
        RETURN;
    END IF;

    IF v_is_primary THEN
        IF p_teacher_id = v_head_teacher_id THEN
            RETURN QUERY
            SELECT DISTINCT d.id, d.name
            FROM study_plan_item spi
            JOIN study_plan sp ON spi.study_plan_id = sp.id
            JOIN discipline d ON spi.discipline_id = d.id
            WHERE sp.parallel_id = (SELECT parallel_id FROM class WHERE id = v_class_id)
              AND sp.academic_year_id = v_latest_academic_year_id
            ORDER BY d.name;
        END IF;
    ELSE
        RETURN QUERY
        SELECT d.id, d.name
        FROM workload w JOIN discipline d ON w.discipline_id = d.id
        WHERE w.class_id = v_class_id
          AND w.teacher_id = p_teacher_id
          AND w.academic_year_id = v_latest_academic_year_id
        ORDER BY d.name;
    END IF;
END;
$$;

ALTER FUNCTION public.get_disciplines_for_student(p_student_id integer, p_teacher_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_distinct_academic_years() RETURNS TABLE(academic_year character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT sp.academic_year
    FROM study_plan sp
    ORDER BY sp.academic_year DESC;
END;
$$;

ALTER FUNCTION public.get_distinct_academic_years() OWNER TO postgres;

CREATE FUNCTION public.get_grades_for_lesson(p_lesson_id integer) RETURNS TABLE(gradebook_id integer, student_id integer, student_full_name text, grade smallint, work_type character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    DECLARE v_class_id INT;
    BEGIN
        SELECT w.class_id INTO v_class_id
        FROM workload w JOIN lesson l ON l.workload_id = w.id
        WHERE l.id = p_lesson_id;
        RETURN QUERY
        SELECT
            gb.id,
            s.id,
            (s.last_name || ' ' || s.first_name || ' ' || COALESCE(s.patronymic, ''))::TEXT,
            gb.grade,
            gb.work_type
        FROM student s
        LEFT JOIN gradebook gb ON gb.student_id = s.id AND gb.lesson_id = p_lesson_id
        WHERE s.class_id = v_class_id
        ORDER BY s.last_name, s.first_name;
    END;
END;
$$;

ALTER FUNCTION public.get_grades_for_lesson(p_lesson_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_grades_for_student_lesson(p_lesson_id integer, p_student_id integer) RETURNS TABLE(gradebook_id integer, grade smallint, work_type character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT
        gb.id,
        gb.grade,
        gb.work_type
    FROM gradebook gb
    WHERE gb.lesson_id = p_lesson_id AND gb.student_id = p_student_id
    ORDER BY gb.grade_time;
END;
$$;

ALTER FUNCTION public.get_grades_for_student_lesson(p_lesson_id integer, p_student_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_journal_data(p_class_id integer, p_discipline_id integer, p_academic_year_id integer) RETURNS TABLE(student_id integer, last_name character varying, first_name character varying, patronymic character varying, lesson_id integer, lesson_number smallint, lesson_date date, topic character varying, gradebook_id integer, grade smallint, work_type character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    WITH
    relevant_workload AS (
        SELECT id FROM workload w
        WHERE w.class_id = p_class_id AND w.discipline_id = p_discipline_id AND w.academic_year_id = p_academic_year_id
    ),
    class_students AS (
        SELECT s.id, s.last_name, s.first_name, s.patronymic
        FROM student s
        WHERE s.class_id = p_class_id AND s.status = 'Active'
    ),
    workload_lessons AS (
        SELECT l.id, l.lesson_number, l.lesson_date, l.topic
        FROM lesson l
        WHERE l.workload_id = (SELECT id FROM relevant_workload)
    )
    SELECT
        cs.id, cs.last_name, cs.first_name, cs.patronymic,
        wl.id, wl.lesson_number, wl.lesson_date, wl.topic,
        COALESCE(gb.id, 0),
        gb.grade, gb.work_type
    FROM class_students cs
    CROSS JOIN workload_lessons wl
    LEFT JOIN gradebook gb ON gb.student_id = cs.id AND gb.lesson_id = wl.id
    ORDER BY cs.last_name, cs.first_name, wl.lesson_number;
END;
$$;

ALTER FUNCTION public.get_journal_data(p_class_id integer, p_discipline_id integer, p_academic_year_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_journal_grades(p_class_id integer, p_discipline_id integer, p_academic_year_id integer) RETURNS TABLE(student_id integer, lesson_id integer, grade smallint, work_type character varying)
    LANGUAGE sql
    AS $$
    SELECT gb.student_id, gb.lesson_id, gb.grade, gb.work_type
    FROM gradebook gb
    JOIN lesson l ON gb.lesson_id = l.id
    JOIN workload w ON l.workload_id = w.id
    WHERE w.class_id = p_class_id
      AND w.discipline_id = p_discipline_id
      AND w.academic_year_id = p_academic_year_id;
$$;

ALTER FUNCTION public.get_journal_grades(p_class_id integer, p_discipline_id integer, p_academic_year_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_journal_lessons(p_class_id integer, p_discipline_id integer, p_academic_year_id integer) RETURNS TABLE(lesson_id integer, lesson_number smallint, lesson_date date, topic character varying)
    LANGUAGE sql
    AS $$
    SELECT l.id, l.lesson_number, l.lesson_date, l.topic
    FROM lesson l
    JOIN workload w ON l.workload_id = w.id
    WHERE w.class_id = p_class_id
      AND w.discipline_id = p_discipline_id
      AND w.academic_year_id = p_academic_year_id
    ORDER BY l.lesson_number;
$$;

ALTER FUNCTION public.get_journal_lessons(p_class_id integer, p_discipline_id integer, p_academic_year_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_journal_lessons(p_class_id integer, p_discipline_id integer, p_academic_year_id integer, p_start_date date, p_end_date date) RETURNS TABLE(lesson_id integer, lesson_number smallint, lesson_date date, topic character varying)
    LANGUAGE sql
    AS $$
    SELECT l.id, l.lesson_number, l.lesson_date, l.topic
    FROM lesson l
    JOIN workload w ON l.workload_id = w.id
    WHERE w.class_id = p_class_id
      AND w.discipline_id = p_discipline_id
      AND w.academic_year_id = p_academic_year_id
      AND (l.lesson_date IS NULL OR l.lesson_date BETWEEN p_start_date AND p_end_date)
    ORDER BY l.lesson_number;
$$;

ALTER FUNCTION public.get_journal_lessons(p_class_id integer, p_discipline_id integer, p_academic_year_id integer, p_start_date date, p_end_date date) OWNER TO postgres;

CREATE FUNCTION public.get_journal_students(p_class_id integer) RETURNS TABLE(student_id integer, last_name character varying, first_name character varying, patronymic character varying)
    LANGUAGE sql
    AS $$
    SELECT s.id, s.last_name, s.first_name, s.patronymic
    FROM student s
    WHERE s.class_id = p_class_id AND s.status = 'Active'
    ORDER BY s.last_name, s.first_name;
$$;

ALTER FUNCTION public.get_journal_students(p_class_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_parents_for_student(p_student_id integer) RETURNS TABLE(id integer, last_name character varying, first_name character varying, patronymic character varying, phone character varying, login character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT p.id, p.last_name, p.first_name, p.patronymic, p.phone, p.login
    FROM parent p
    JOIN student_parent sp ON p.id = sp.parent_id
    WHERE sp.student_id = p_student_id
    ORDER BY p.last_name, p.first_name;
END;
$$;

ALTER FUNCTION public.get_parents_for_student(p_student_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_student_average_grades(p_student_id integer) RETURNS TABLE(discipline_name character varying, average_grade_value numeric)
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_latest_academic_year_id INT;
BEGIN
    SELECT wl.academic_year_id INTO v_latest_academic_year_id
    FROM gradebook gb
    JOIN lesson l ON gb.lesson_id = l.id
    JOIN workload wl ON l.workload_id = wl.id
    JOIN academic_year ay ON wl.academic_year_id = ay.id
    WHERE gb.student_id = p_student_id
    ORDER BY ay.start_date DESC
    LIMIT 1;

    IF v_latest_academic_year_id IS NULL THEN RETURN; END IF;

    RETURN QUERY
    SELECT
        d.name,
        ROUND(AVG(gb.grade), 2)
    FROM gradebook gb
    JOIN lesson l ON gb.lesson_id = l.id
    JOIN workload wl ON l.workload_id = wl.id
    JOIN discipline d ON wl.discipline_id = d.id
    WHERE gb.student_id = p_student_id AND wl.academic_year_id = v_latest_academic_year_id
    GROUP BY d.name
    ORDER BY d.name;
END;
$$;

ALTER FUNCTION public.get_student_average_grades(p_student_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_student_lessons_and_grades(p_student_id integer, p_discipline_id integer) RETURNS TABLE(lesson_id integer, lesson_number smallint, lesson_date date, lesson_topic character varying, grades_line text)
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_class_id INT;
    v_is_primary BOOLEAN;
    v_latest_academic_year_id INT;
    v_workload_id INT;
BEGIN
    SELECT s.class_id, c.is_primary_school INTO v_class_id, v_is_primary
    FROM student s JOIN class c ON s.class_id = c.id
    WHERE s.id = p_student_id;
    SELECT ay.id INTO v_latest_academic_year_id
    FROM academic_year ay WHERE ay.status = 'Current' LIMIT 1;

    IF v_latest_academic_year_id IS NULL THEN RETURN; END IF;
    SELECT w.id INTO v_workload_id
    FROM workload w
    WHERE w.class_id = v_class_id
      AND w.discipline_id = p_discipline_id
      AND w.academic_year_id = v_latest_academic_year_id
    LIMIT 1;
    IF v_workload_id IS NULL AND v_is_primary THEN
        v_workload_id := find_or_create_workload_for_primary(p_student_id, p_discipline_id);
    END IF;

    IF v_workload_id IS NULL THEN RETURN; END IF;
    RETURN QUERY
    WITH lesson_grades AS (
        SELECT
            gb.lesson_id,
            STRING_AGG(
                CASE WHEN gb.work_type = 'Н' THEN 'Н' ELSE gb.grade::text END || ' (' || gb.work_type || ')',
                '; ' ORDER BY gb.grade_time
            ) as aggregated_grades
        FROM gradebook gb
        WHERE gb.student_id = p_student_id
          AND gb.lesson_id IN (SELECT id FROM lesson WHERE workload_id = v_workload_id)
        GROUP BY gb.lesson_id
    )
    SELECT
        l.id,
        l.lesson_number,
        l.lesson_date,
        l.topic,
        lg.aggregated_grades
    FROM lesson l
    LEFT JOIN lesson_grades lg ON l.id = lg.lesson_id
    WHERE l.workload_id = v_workload_id
    ORDER BY l.lesson_number;

END;
$$;

ALTER FUNCTION public.get_student_lessons_and_grades(p_student_id integer, p_discipline_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_student_movement_report(p_start_date date, p_end_date date) RETURNS jsonb
    LANGUAGE plpgsql
    AS $$
DECLARE
    result jsonb;
BEGIN
    WITH
    summary AS (
        SELECT
            COUNT(*) FILTER (WHERE enrollment_date < p_start_date AND (status <> 'Expelled' OR status_change_date >= p_start_date)) as total_at_start,
            COUNT(*) FILTER (WHERE enrollment_date BETWEEN p_start_date AND p_end_date) as arrived_count,
            COUNT(*) FILTER (WHERE status = 'Expelled' AND status_change_date BETWEEN p_start_date AND p_end_date) as departed_count
        FROM student
    ),
    arrived_students AS (
        SELECT jsonb_agg(
            jsonb_build_object(
                'fullName', s.last_name || ' ' || s.first_name || ' ' || COALESCE(s.patronymic, ''),
                'enrollmentDate', s.enrollment_date,
                'className', p.number || ' "' || c.letter || '"'
            ) ORDER BY s.enrollment_date
        ) as students
        FROM student s
        JOIN class c ON s.class_id = c.id
        JOIN parallel p ON c.parallel_id = p.id
        WHERE s.enrollment_date BETWEEN p_start_date AND p_end_date
    ),
    departed_students AS (
        SELECT jsonb_agg(
            jsonb_build_object(
                'fullName', s.last_name || ' ' || s.first_name || ' ' || COALESCE(s.patronymic, ''),
                'departureDate', s.status_change_date,
                'className', p.number || ' "' || c.letter || '"'
            ) ORDER BY s.status_change_date
        ) as students
        FROM student s
        JOIN class c ON s.class_id = c.id
        JOIN parallel p ON c.parallel_id = p.id
        WHERE s.status = 'Expelled' AND s.status_change_date BETWEEN p_start_date AND p_end_date
    )
    SELECT jsonb_build_object(
        'totalAtStart', COALESCE((SELECT total_at_start FROM summary), 0),
        'arrivedCount', COALESCE((SELECT arrived_count FROM summary), 0),
        'departedCount', COALESCE((SELECT departed_count FROM summary), 0),
        'totalAtEnd', COALESCE((SELECT total_at_start + arrived_count - departed_count FROM summary), 0),
        'arrivedStudents', COALESCE((SELECT students FROM arrived_students), '[]'::jsonb),
        'departedStudents', COALESCE((SELECT students FROM departed_students), '[]'::jsonb)
    ) INTO result;

    RETURN result;
END;
$$;

ALTER FUNCTION public.get_student_movement_report(p_start_date date, p_end_date date) OWNER TO postgres;

CREATE FUNCTION public.get_study_plan_items(p_study_plan_id integer) RETURNS TABLE(id integer, discipline_id integer, discipline_name character varying, lessons_count smallint)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT
        spi.id,
        spi.discipline_id,
        d.name,
        spi.lessons_count
    FROM study_plan_item spi
    JOIN discipline d ON spi.discipline_id = d.id
    WHERE spi.study_plan_id = p_study_plan_id
    ORDER BY d.name;
END;
$$;

ALTER FUNCTION public.get_study_plan_items(p_study_plan_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_teacher_workload_report(p_academic_year_id integer) RETURNS TABLE(teacher_id integer, teacher_full_name text, total_lessons_count bigint, workload_details text)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    WITH teacher_workload AS (
        SELECT
            t.id as t_id,
            (t.last_name || ' ' || t.first_name || ' ' || COALESCE(t.patronymic, ''))::text as t_full_name,
            d.name as discipline_name,
            (p.number || ' "' || c.letter || '"')::text as class_name,
            spi.lessons_count
        FROM workload w
        JOIN teacher t ON w.teacher_id = t.id
        JOIN class c ON w.class_id = c.id
        JOIN parallel p ON c.parallel_id = p.id
        JOIN discipline d ON w.discipline_id = d.id
        JOIN study_plan sp ON sp.parallel_id = p.id AND sp.academic_year_id = w.academic_year_id
        JOIN study_plan_item spi ON spi.study_plan_id = sp.id AND spi.discipline_id = w.discipline_id
        WHERE w.academic_year_id = p_academic_year_id
    )
    SELECT
        tw.t_id,
        tw.t_full_name,
        SUM(tw.lessons_count) as total_lessons_count,
        STRING_AGG(tw.class_name || ' - ' || tw.discipline_name || ' (' || tw.lessons_count || ' з.)', '; ' ORDER BY tw.class_name, tw.discipline_name) as workload_details
    FROM teacher_workload tw
    GROUP BY tw.t_id, tw.t_full_name
    ORDER BY tw.t_full_name;
END;
$$;

ALTER FUNCTION public.get_teacher_workload_report(p_academic_year_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_user_auth_data_by_login(p_login character varying) RETURNS TABLE(id integer, full_name text, role character varying, related_student_id integer, password_plain character varying, password_hash bytea, password_salt bytea)
    LANGUAGE plpgsql
    AS $$
    BEGIN
        RETURN QUERY
        SELECT
            t.id, concat_ws(' ', t.last_name, t.first_name, t.patronymic)::text, t.role,
            NULL::integer, NULL::character varying, t.password_hash, t.password_salt
        FROM teacher t WHERE t.login = p_login
        UNION ALL
        SELECT
            s.id, concat_ws(' ', s.last_name, s.first_name, s.patronymic)::text, 'student'::varchar,
            s.id, NULL::character varying, s.password_hash, s.password_salt
        FROM student s WHERE s.login = p_login
        UNION ALL
        SELECT
            p.id, concat_ws(' ', p.last_name, p.first_name, p.patronymic)::text, 'parent'::varchar,
            (SELECT sp.student_id FROM student_parent sp WHERE sp.parent_id = p.id LIMIT 1),
            NULL::character varying, p.password_hash, p.password_salt
        FROM parent p WHERE p.login = p_login;
    END;
    $$;

ALTER FUNCTION public.get_user_auth_data_by_login(p_login character varying) OWNER TO postgres;

CREATE FUNCTION public.get_workload_for_class(p_class_id integer, p_academic_year_id integer) RETURNS TABLE(discipline_id integer, discipline_name character varying, lessons_count smallint, teacher_id integer, teacher_full_name text)
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_is_primary BOOLEAN;
    v_head_teacher_id INT;
BEGIN
    SELECT c.is_primary_school, c.head_teacher_id INTO v_is_primary, v_head_teacher_id
    FROM class c WHERE c.id = p_class_id;
    IF v_is_primary THEN
        RETURN QUERY SELECT spi.discipline_id, d.name::varchar, spi.lessons_count, c.head_teacher_id,
               (SELECT concat_ws(' ', t.last_name, t.first_name, t.patronymic)::text FROM teacher t WHERE t.id = c.head_teacher_id)
        FROM study_plan_item spi
        JOIN study_plan sp ON spi.study_plan_id = sp.id
        JOIN discipline d ON spi.discipline_id = d.id
        JOIN class c ON sp.parallel_id = c.parallel_id
        WHERE c.id = p_class_id AND sp.academic_year_id = p_academic_year_id ORDER BY d.name;
    ELSE
        RETURN QUERY SELECT spi.discipline_id, d.name::varchar, spi.lessons_count, wl.teacher_id,
               concat_ws(' ', t.last_name, t.first_name, t.patronymic)::text
        FROM study_plan_item spi
        JOIN study_plan sp ON spi.study_plan_id = sp.id
        JOIN discipline d ON spi.discipline_id = d.id
        LEFT JOIN workload wl ON wl.class_id = p_class_id AND wl.discipline_id = spi.discipline_id AND wl.academic_year_id = p_academic_year_id
        LEFT JOIN teacher t ON wl.teacher_id = t.id
        WHERE sp.parallel_id = (SELECT parallel_id FROM class WHERE id = p_class_id)
          AND sp.academic_year_id = p_academic_year_id ORDER BY d.name;
    END IF;
END; $$;

ALTER FUNCTION public.get_workload_for_class(p_class_id integer, p_academic_year_id integer) OWNER TO postgres;

CREATE FUNCTION public.get_workload_for_class(p_class_id integer, p_academic_year character varying) RETURNS TABLE(discipline_id integer, discipline_name character varying, lessons_count smallint, teacher_id integer, teacher_full_name text)
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_is_primary BOOLEAN;
    v_head_teacher_id INT;
BEGIN
    SELECT c.is_primary_school, c.head_teacher_id INTO v_is_primary, v_head_teacher_id
    FROM class c WHERE c.id = p_class_id;

    IF v_is_primary THEN
        RETURN QUERY
        SELECT
            spi.discipline_id,
            d.name::varchar,
            spi.lessons_count,
            c.head_teacher_id,
            (SELECT concat_ws(' ', t.last_name, t.first_name, t.patronymic)::text FROM teacher t WHERE t.id = c.head_teacher_id)
        FROM study_plan_item spi
        JOIN study_plan sp ON spi.study_plan_id = sp.id
        JOIN discipline d ON spi.discipline_id = d.id
        JOIN class c ON sp.parallel_id = c.parallel_id
        WHERE c.id = p_class_id AND sp.academic_year = p_academic_year
        ORDER BY d.name;
    ELSE
        RETURN QUERY
        SELECT
            spi.discipline_id,
            d.name::varchar,
            spi.lessons_count,
            wl.teacher_id,
            concat_ws(' ', t.last_name, t.first_name, t.patronymic)::text
        FROM study_plan_item spi
        JOIN study_plan sp ON spi.study_plan_id = sp.id
        JOIN discipline d ON spi.discipline_id = d.id
        LEFT JOIN workload wl ON wl.class_id = p_class_id AND wl.discipline_id = spi.discipline_id AND wl.academic_year = p_academic_year
        LEFT JOIN teacher t ON wl.teacher_id = t.id
        WHERE sp.parallel_id = (SELECT parallel_id FROM class WHERE id = p_class_id)
          AND sp.academic_year = p_academic_year
        ORDER BY d.name;
    END IF;
END;
$$;

ALTER FUNCTION public.get_workload_for_class(p_class_id integer, p_academic_year character varying) OWNER TO postgres;

CREATE FUNCTION public.link_student_to_parent(p_student_id integer, p_parent_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO student_parent(student_id, parent_id)
    VALUES (p_student_id, p_parent_id)
    ON CONFLICT DO NOTHING;
END;
$$;

ALTER FUNCTION public.link_student_to_parent(p_student_id integer, p_parent_id integer) OWNER TO postgres;

CREATE FUNCTION public.log_gradebook_changes() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_workload_id INT;
BEGIN
    IF (TG_OP = 'INSERT' OR TG_OP = 'UPDATE') THEN
        SELECT l.workload_id INTO v_workload_id FROM lesson l WHERE l.id = NEW.lesson_id;
    END IF;
    IF (TG_OP = 'DELETE') THEN
        SELECT l.workload_id INTO v_workload_id FROM lesson l WHERE l.id = OLD.lesson_id;
    END IF;
    IF (TG_OP = 'INSERT') THEN
        INSERT INTO gradebook_audit_log (action_type, gradebook_id, student_id, workload_id, new_grade)
        VALUES ('INSERT', NEW.id, NEW.student_id, v_workload_id, NEW.grade);
        RETURN NEW;
    ELSIF (TG_OP = 'UPDATE') THEN
        IF OLD.grade IS DISTINCT FROM NEW.grade THEN
            INSERT INTO gradebook_audit_log (action_type, gradebook_id, student_id, workload_id, old_grade, new_grade)
            VALUES ('UPDATE', NEW.id, NEW.student_id, v_workload_id, OLD.grade, NEW.grade);
        END IF;
        RETURN NEW;
    ELSIF (TG_OP = 'DELETE') THEN
        INSERT INTO gradebook_audit_log (action_type, gradebook_id, student_id, workload_id, old_grade)
        VALUES ('DELETE', OLD.id, OLD.student_id, v_workload_id, OLD.grade);
        RETURN OLD;
    END IF;
    RETURN NULL;
END;
$$;

ALTER FUNCTION public.log_gradebook_changes() OWNER TO postgres;

CREATE FUNCTION public.promote_students_to_next_year(p_completed_year_id integer, p_next_year_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_student RECORD;
    v_current_class RECORD;
    v_next_parallel_number INT;
    v_next_parallel_id INT;
    v_next_class_id INT;
BEGIN
    UPDATE academic_year SET status = 'Archived' WHERE id = p_completed_year_id;
    UPDATE academic_year SET status = 'Current' WHERE id = p_next_year_id;
    FOR v_student IN SELECT id, class_id FROM student WHERE status = 'Active' LOOP
        SELECT c.id, c.letter, p.number INTO v_current_class
        FROM class c JOIN parallel p ON c.parallel_id = p.id
        WHERE c.id = v_student.class_id;
        
        IF v_current_class.number >= 11 THEN
            UPDATE student SET status = 'Graduated' WHERE id = v_student.id;
            CONTINUE;
        END IF;
        
        v_next_parallel_number := v_current_class.number + 1;
        SELECT id INTO v_next_parallel_id FROM parallel WHERE "number" = v_next_parallel_number;
        SELECT id INTO v_next_class_id FROM class WHERE parallel_id = v_next_parallel_id AND letter = v_current_class.letter;
        
        IF v_next_class_id IS NULL THEN
            INSERT INTO class (letter, parallel_id, head_teacher_id)
            VALUES (v_current_class.letter, v_next_parallel_id, NULL)
            RETURNING id INTO v_next_class_id;
        END IF;
        
        UPDATE student SET class_id = v_next_class_id WHERE id = v_student.id;
    END LOOP;
END;
$$;

ALTER FUNCTION public.promote_students_to_next_year(p_completed_year_id integer, p_next_year_id integer) OWNER TO postgres;

CREATE FUNCTION public.transfer_student(p_student_id integer, p_new_class_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE student SET class_id = p_new_class_id WHERE id = p_student_id;
END;
$$;

ALTER FUNCTION public.transfer_student(p_student_id integer, p_new_class_id integer) OWNER TO postgres;

CREATE FUNCTION public.unlink_student_from_parent(p_student_id integer, p_parent_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM student_parent
    WHERE student_id = p_student_id AND parent_id = p_parent_id;
END;
$$;

ALTER FUNCTION public.unlink_student_from_parent(p_student_id integer, p_parent_id integer) OWNER TO postgres;

CREATE FUNCTION public.update_achievement(p_id integer, p_event_name text, p_event_date timestamp without time zone, p_level text, p_place integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN UPDATE achievement SET event_name = p_event_name, event_date = p_event_date, level = p_level, place = p_place WHERE id = p_id; END;
$$;

ALTER FUNCTION public.update_achievement(p_id integer, p_event_name text, p_event_date timestamp without time zone, p_level text, p_place integer) OWNER TO postgres;

CREATE FUNCTION public.update_class_head_teacher(p_class_id integer, p_new_head_teacher_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE class SET head_teacher_id = p_new_head_teacher_id WHERE id = p_class_id;
END;
$$;

ALTER FUNCTION public.update_class_head_teacher(p_class_id integer, p_new_head_teacher_id integer) OWNER TO postgres;

CREATE FUNCTION public.update_grade(p_gradebook_id integer, p_new_grade integer, p_new_work_type text) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN UPDATE gradebook SET grade = p_new_grade, work_type = p_new_work_type, grade_time = now()::time WHERE id = p_gradebook_id; END;
$$;

ALTER FUNCTION public.update_grade(p_gradebook_id integer, p_new_grade integer, p_new_work_type text) OWNER TO postgres;

CREATE FUNCTION public.update_lesson_details(p_lesson_id integer, p_lesson_date date, p_topic text) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE lesson 
    SET lesson_date = p_lesson_date, topic = p_topic
    WHERE id = p_lesson_id;
END;
$$;

ALTER FUNCTION public.update_lesson_details(p_lesson_id integer, p_lesson_date date, p_topic text) OWNER TO postgres;

CREATE FUNCTION public.update_parent(p_id integer, p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_phone character varying, p_email character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE parent SET
        last_name = p_last_name,
        first_name = p_first_name,
        patronymic = p_patronymic,
        phone = p_phone,
        email = p_email
    WHERE id = p_id;
END;
$$;

ALTER FUNCTION public.update_parent(p_id integer, p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_phone character varying, p_email character varying) OWNER TO postgres;

CREATE FUNCTION public.update_primary_school_flag() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_parallel_number SMALLINT;
BEGIN
    SELECT p.number INTO v_parallel_number FROM parallel p WHERE p.id = NEW.parallel_id;
    IF v_parallel_number <= 3 THEN
        NEW.is_primary_school := TRUE;
    ELSE
        NEW.is_primary_school := FALSE;
    END IF;

    RETURN NEW;
END;
$$;

ALTER FUNCTION public.update_primary_school_flag() OWNER TO postgres;

CREATE FUNCTION public.update_student(p_id integer, p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_class_id integer, p_birth_date date, p_notes text) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE student
    SET last_name = p_last_name,
        first_name = p_first_name,
        patronymic = p_patronymic,
        class_id = p_class_id,
        birth_date = p_birth_date,
        notes = p_notes
    WHERE id = p_id;
END;
$$;

ALTER FUNCTION public.update_student(p_id integer, p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_class_id integer, p_birth_date date, p_notes text) OWNER TO postgres;

CREATE FUNCTION public.update_teacher(p_id integer, p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_phone character varying, p_email character varying, p_login character varying, p_role character varying, p_notes text) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_current_role VARCHAR;
    v_admin_count INT;
BEGIN
    SELECT role INTO v_current_role FROM teacher WHERE id = p_id;
    IF v_current_role = 'admin' AND p_role <> 'admin' THEN
        SELECT count(*) INTO v_admin_count FROM teacher WHERE role = 'admin';
        IF v_admin_count <= 1 THEN
            RAISE EXCEPTION 'Невозможно изменить роль последнего администратора. В системе должен оставаться хотя бы один администратор.';
        END IF;
    END IF;
    UPDATE teacher SET
        last_name = p_last_name,
        first_name = p_first_name,
        patronymic = p_patronymic,
        phone = p_phone,
        email = p_email,
        login = p_login,
        role = p_role,
        notes = p_notes
    WHERE id = p_id;
END;
$$;

ALTER FUNCTION public.update_teacher(p_id integer, p_last_name character varying, p_first_name character varying, p_patronymic character varying, p_phone character varying, p_email character varying, p_login character varying, p_role character varying, p_notes text) OWNER TO postgres;

CREATE FUNCTION public.upsert_study_plan_item(p_study_plan_id integer, p_discipline_id integer, p_lessons_count integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO study_plan_item (study_plan_id, discipline_id, lessons_count)
    VALUES (p_study_plan_id, p_discipline_id, p_lessons_count)
    ON CONFLICT (study_plan_id, discipline_id)
    DO UPDATE SET lessons_count = p_lessons_count;
END;
$$;

ALTER FUNCTION public.upsert_study_plan_item(p_study_plan_id integer, p_discipline_id integer, p_lessons_count integer) OWNER TO postgres;

CREATE FUNCTION public.upsert_workload(p_class_id integer, p_discipline_id integer, p_teacher_id integer, p_academic_year_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE v_workload_id INT; v_lessons_count INT;
BEGIN
    INSERT INTO workload (class_id, discipline_id, teacher_id, academic_year_id)
    VALUES (p_class_id, p_discipline_id, p_teacher_id, p_academic_year_id)
    ON CONFLICT (class_id, discipline_id, academic_year_id) DO UPDATE SET teacher_id = p_teacher_id RETURNING id INTO v_workload_id;
    IF NOT EXISTS (SELECT 1 FROM lesson WHERE workload_id = v_workload_id) THEN
        SELECT spi.lessons_count INTO v_lessons_count
        FROM study_plan_item spi
        JOIN study_plan sp ON spi.study_plan_id = sp.id JOIN class c ON sp.parallel_id = c.parallel_id
        WHERE c.id = p_class_id AND spi.discipline_id = p_discipline_id AND sp.academic_year_id = p_academic_year_id;
        IF v_lessons_count > 0 THEN
            FOR i IN 1..v_lessons_count LOOP
                INSERT INTO lesson (workload_id, lesson_number) VALUES (v_workload_id, i);
            END LOOP;
        END IF;
    END IF;
END; $$;

ALTER FUNCTION public.upsert_workload(p_class_id integer, p_discipline_id integer, p_teacher_id integer, p_academic_year_id integer) OWNER TO postgres;

CREATE FUNCTION public.upsert_workload(p_class_id integer, p_discipline_id integer, p_teacher_id integer, p_academic_year character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_workload_id INT;
    v_lessons_count INT;
BEGIN
    INSERT INTO workload (class_id, discipline_id, teacher_id, academic_year)
    VALUES (p_class_id, p_discipline_id, p_teacher_id, p_academic_year)
    ON CONFLICT (class_id, discipline_id, academic_year)
    DO UPDATE SET teacher_id = p_teacher_id
    RETURNING id INTO v_workload_id;

    IF NOT EXISTS (SELECT 1 FROM lesson WHERE workload_id = v_workload_id) THEN
        SELECT spi.lessons_count INTO v_lessons_count
        FROM study_plan_item spi
        JOIN study_plan sp ON spi.study_plan_id = sp.id
        JOIN class c ON sp.parallel_id = c.parallel_id
        WHERE c.id = p_class_id AND spi.discipline_id = p_discipline_id AND sp.academic_year = p_academic_year;
        
        IF v_lessons_count > 0 THEN
            FOR i IN 1..v_lessons_count LOOP
                INSERT INTO lesson (workload_id, lesson_number) VALUES (v_workload_id, i);
            END LOOP;
        END IF;
    END IF;
END;
$$;

ALTER FUNCTION public.upsert_workload(p_class_id integer, p_discipline_id integer, p_teacher_id integer, p_academic_year character varying) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

CREATE TABLE public.academic_year (
    id integer NOT NULL,
    name character varying(9) NOT NULL,
    start_date date NOT NULL,
    end_date date NOT NULL,
    status character varying(20) DEFAULT 'Upcoming'::character varying NOT NULL,
    CONSTRAINT academic_year_status_check CHECK (((status)::text = ANY (ARRAY[('Upcoming'::character varying)::text, ('Current'::character varying)::text, ('Archived'::character varying)::text])))
);

ALTER TABLE public.academic_year OWNER TO postgres;

CREATE SEQUENCE public.academic_year_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.academic_year_id_seq OWNER TO postgres;

ALTER SEQUENCE public.academic_year_id_seq OWNED BY public.academic_year.id;

CREATE TABLE public.achievement (
    id integer NOT NULL,
    student_id integer NOT NULL,
    event_name character varying(255) NOT NULL,
    event_date date,
    level character varying(100),
    place smallint
);

ALTER TABLE public.achievement OWNER TO postgres;

CREATE SEQUENCE public.achievement_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.achievement_id_seq OWNER TO postgres;

ALTER SEQUENCE public.achievement_id_seq OWNED BY public.achievement.id;

CREATE TABLE public.class (
    id integer NOT NULL,
    letter character(1) NOT NULL,
    parallel_id integer NOT NULL,
    head_teacher_id integer,
    is_primary_school boolean DEFAULT false NOT NULL
);

ALTER TABLE public.class OWNER TO postgres;

CREATE SEQUENCE public.class_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.class_id_seq OWNER TO postgres;

ALTER SEQUENCE public.class_id_seq OWNED BY public.class.id;

CREATE TABLE public.discipline (
    id integer NOT NULL,
    name character varying(150) NOT NULL
);

ALTER TABLE public.discipline OWNER TO postgres;

CREATE SEQUENCE public.discipline_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.discipline_id_seq OWNER TO postgres;

ALTER SEQUENCE public.discipline_id_seq OWNED BY public.discipline.id;

CREATE TABLE public.gradebook (
    id integer NOT NULL,
    student_id integer NOT NULL,
    grade smallint,
    work_type character varying(100),
    grade_date date NOT NULL,
    grade_time time without time zone,
    lesson_id integer
);

ALTER TABLE public.gradebook OWNER TO postgres;

CREATE TABLE public.gradebook_audit_log (
    id integer NOT NULL,
    action_type character varying(10) NOT NULL,
    gradebook_id integer,
    student_id integer,
    workload_id integer,
    old_grade smallint,
    new_grade smallint,
    changed_by_user name DEFAULT CURRENT_USER NOT NULL,
    changed_at timestamp with time zone DEFAULT now() NOT NULL
);

ALTER TABLE public.gradebook_audit_log OWNER TO postgres;

CREATE SEQUENCE public.gradebook_audit_log_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.gradebook_audit_log_id_seq OWNER TO postgres;

ALTER SEQUENCE public.gradebook_audit_log_id_seq OWNED BY public.gradebook_audit_log.id;

CREATE SEQUENCE public.gradebook_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.gradebook_id_seq OWNER TO postgres;

ALTER SEQUENCE public.gradebook_id_seq OWNED BY public.gradebook.id;

CREATE TABLE public.lesson (
    id integer NOT NULL,
    workload_id integer NOT NULL,
    lesson_number smallint NOT NULL,
    topic character varying(255) DEFAULT 'Тема не указана'::character varying,
    homework text,
    lesson_date date
);

ALTER TABLE public.lesson OWNER TO postgres;

CREATE SEQUENCE public.lesson_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.lesson_id_seq OWNER TO postgres;

ALTER SEQUENCE public.lesson_id_seq OWNED BY public.lesson.id;

CREATE TABLE public.parallel (
    id integer NOT NULL,
    number smallint NOT NULL,
    CONSTRAINT parallel_number_check CHECK (((number > 0) AND (number < 12)))
);

ALTER TABLE public.parallel OWNER TO postgres;

CREATE SEQUENCE public.parallel_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.parallel_id_seq OWNER TO postgres;

ALTER SEQUENCE public.parallel_id_seq OWNED BY public.parallel.id;

CREATE TABLE public.parent (
    id integer NOT NULL,
    last_name character varying(100) NOT NULL,
    first_name character varying(100) NOT NULL,
    patronymic character varying(100),
    phone character varying(20),
    email character varying(100),
    login character varying(50) NOT NULL,
    password_hash bytea,
    password_salt bytea
);

ALTER TABLE public.parent OWNER TO postgres;

CREATE SEQUENCE public.parent_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.parent_id_seq OWNER TO postgres;

ALTER SEQUENCE public.parent_id_seq OWNED BY public.parent.id;

CREATE TABLE public.student (
    id integer NOT NULL,
    last_name character varying(100) NOT NULL,
    first_name character varying(100) NOT NULL,
    patronymic character varying(100),
    class_id integer NOT NULL,
    enrollment_year smallint NOT NULL,
    birth_date date NOT NULL,
    gender character(1) NOT NULL,
    notes text,
    login character varying(50),
    password_hash bytea,
    password_salt bytea,
    status character varying(20) DEFAULT 'Active'::character varying NOT NULL,
    enrollment_date date,
    status_change_date date,
    CONSTRAINT student_gender_check CHECK ((gender = ANY (ARRAY['М'::bpchar, 'Ж'::bpchar]))),
    CONSTRAINT student_status_check CHECK (((status)::text = ANY (ARRAY[('Active'::character varying)::text, ('Graduated'::character varying)::text, ('Expelled'::character varying)::text])))
);

ALTER TABLE public.student OWNER TO postgres;

COMMENT ON COLUMN public.student.enrollment_date IS 'Точная дата зачисления в школу';

COMMENT ON COLUMN public.student.status_change_date IS 'Дата последнего изменения статуса (зачислен, отчислен, выпустился)';

CREATE SEQUENCE public.student_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.student_id_seq OWNER TO postgres;

ALTER SEQUENCE public.student_id_seq OWNED BY public.student.id;

CREATE TABLE public.student_parent (
    student_id integer NOT NULL,
    parent_id integer NOT NULL
);

ALTER TABLE public.student_parent OWNER TO postgres;

CREATE TABLE public.study_plan (
    id integer NOT NULL,
    name character varying(200) NOT NULL,
    parallel_id integer NOT NULL,
    academic_year_id integer NOT NULL
);

ALTER TABLE public.study_plan OWNER TO postgres;

CREATE SEQUENCE public.study_plan_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.study_plan_id_seq OWNER TO postgres;

ALTER SEQUENCE public.study_plan_id_seq OWNED BY public.study_plan.id;

CREATE TABLE public.study_plan_item (
    id integer NOT NULL,
    study_plan_id integer NOT NULL,
    discipline_id integer NOT NULL,
    lessons_count smallint NOT NULL,
    CONSTRAINT study_plan_item_hours_check CHECK ((lessons_count > 0))
);

ALTER TABLE public.study_plan_item OWNER TO postgres;

CREATE SEQUENCE public.study_plan_item_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.study_plan_item_id_seq OWNER TO postgres;

ALTER SEQUENCE public.study_plan_item_id_seq OWNED BY public.study_plan_item.id;

CREATE TABLE public.teacher (
    id integer NOT NULL,
    last_name character varying(100) NOT NULL,
    first_name character varying(100) NOT NULL,
    patronymic character varying(100),
    phone character varying(20),
    email character varying(100),
    notes text,
    login character varying(50) NOT NULL,
    role character varying(20) DEFAULT 'teacher'::character varying NOT NULL,
    password_hash bytea,
    password_salt bytea,
    CONSTRAINT teacher_role_check CHECK (((role)::text = ANY (ARRAY[('teacher'::character varying)::text, ('admin'::character varying)::text])))
);

ALTER TABLE public.teacher OWNER TO postgres;

CREATE SEQUENCE public.teacher_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.teacher_id_seq OWNER TO postgres;

ALTER SEQUENCE public.teacher_id_seq OWNED BY public.teacher.id;

CREATE TABLE public.workload (
    id integer NOT NULL,
    teacher_id integer NOT NULL,
    discipline_id integer NOT NULL,
    class_id integer NOT NULL,
    academic_year_id integer NOT NULL
);

ALTER TABLE public.workload OWNER TO postgres;

CREATE SEQUENCE public.workload_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.workload_id_seq OWNER TO postgres;

ALTER SEQUENCE public.workload_id_seq OWNED BY public.workload.id;

ALTER TABLE ONLY public.academic_year ALTER COLUMN id SET DEFAULT nextval('public.academic_year_id_seq'::regclass);

ALTER TABLE ONLY public.achievement ALTER COLUMN id SET DEFAULT nextval('public.achievement_id_seq'::regclass);

ALTER TABLE ONLY public.class ALTER COLUMN id SET DEFAULT nextval('public.class_id_seq'::regclass);

ALTER TABLE ONLY public.discipline ALTER COLUMN id SET DEFAULT nextval('public.discipline_id_seq'::regclass);

ALTER TABLE ONLY public.gradebook ALTER COLUMN id SET DEFAULT nextval('public.gradebook_id_seq'::regclass);

ALTER TABLE ONLY public.gradebook_audit_log ALTER COLUMN id SET DEFAULT nextval('public.gradebook_audit_log_id_seq'::regclass);

ALTER TABLE ONLY public.lesson ALTER COLUMN id SET DEFAULT nextval('public.lesson_id_seq'::regclass);

ALTER TABLE ONLY public.parallel ALTER COLUMN id SET DEFAULT nextval('public.parallel_id_seq'::regclass);

ALTER TABLE ONLY public.parent ALTER COLUMN id SET DEFAULT nextval('public.parent_id_seq'::regclass);

ALTER TABLE ONLY public.student ALTER COLUMN id SET DEFAULT nextval('public.student_id_seq'::regclass);

ALTER TABLE ONLY public.study_plan ALTER COLUMN id SET DEFAULT nextval('public.study_plan_id_seq'::regclass);

ALTER TABLE ONLY public.study_plan_item ALTER COLUMN id SET DEFAULT nextval('public.study_plan_item_id_seq'::regclass);

ALTER TABLE ONLY public.teacher ALTER COLUMN id SET DEFAULT nextval('public.teacher_id_seq'::regclass);

ALTER TABLE ONLY public.workload ALTER COLUMN id SET DEFAULT nextval('public.workload_id_seq'::regclass);

ALTER TABLE ONLY public.academic_year
    ADD CONSTRAINT academic_year_name_key UNIQUE (name);

ALTER TABLE ONLY public.academic_year
    ADD CONSTRAINT academic_year_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.achievement
    ADD CONSTRAINT achievement_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.class
    ADD CONSTRAINT class_parallel_id_letter_key UNIQUE (parallel_id, letter);

ALTER TABLE ONLY public.class
    ADD CONSTRAINT class_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.discipline
    ADD CONSTRAINT discipline_name_key UNIQUE (name);

ALTER TABLE ONLY public.discipline
    ADD CONSTRAINT discipline_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.gradebook_audit_log
    ADD CONSTRAINT gradebook_audit_log_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.gradebook
    ADD CONSTRAINT gradebook_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.lesson
    ADD CONSTRAINT lesson_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.parallel
    ADD CONSTRAINT parallel_number_key UNIQUE (number);

ALTER TABLE ONLY public.parallel
    ADD CONSTRAINT parallel_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.parent
    ADD CONSTRAINT parent_email_key UNIQUE (email);

ALTER TABLE ONLY public.parent
    ADD CONSTRAINT parent_login_key UNIQUE (login);

ALTER TABLE ONLY public.parent
    ADD CONSTRAINT parent_phone_key UNIQUE (phone);

ALTER TABLE ONLY public.parent
    ADD CONSTRAINT parent_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.student
    ADD CONSTRAINT student_login_key UNIQUE (login);

ALTER TABLE ONLY public.student_parent
    ADD CONSTRAINT student_parent_pkey PRIMARY KEY (student_id, parent_id);

ALTER TABLE ONLY public.student
    ADD CONSTRAINT student_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.study_plan_item
    ADD CONSTRAINT study_plan_item_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.study_plan_item
    ADD CONSTRAINT study_plan_item_plan_discipline_key UNIQUE (study_plan_id, discipline_id);

ALTER TABLE ONLY public.study_plan
    ADD CONSTRAINT study_plan_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.teacher
    ADD CONSTRAINT teacher_email_key UNIQUE (email);

ALTER TABLE ONLY public.teacher
    ADD CONSTRAINT teacher_login_key UNIQUE (login);

ALTER TABLE ONLY public.teacher
    ADD CONSTRAINT teacher_phone_key UNIQUE (phone);

ALTER TABLE ONLY public.teacher
    ADD CONSTRAINT teacher_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.class
    ADD CONSTRAINT uq_class_head_teacher UNIQUE (head_teacher_id);

ALTER TABLE ONLY public.lesson
    ADD CONSTRAINT uq_lesson_workload_number UNIQUE (workload_id, lesson_number);

ALTER TABLE ONLY public.workload
    ADD CONSTRAINT workload_class_discipline_year_id_key UNIQUE (class_id, discipline_id, academic_year_id);

ALTER TABLE ONLY public.workload
    ADD CONSTRAINT workload_pkey PRIMARY KEY (id);

CREATE INDEX idx_achievement_student_id ON public.achievement USING btree (student_id);

CREATE INDEX idx_gradebook_lesson_id ON public.gradebook USING btree (lesson_id);

CREATE INDEX idx_gradebook_student_id ON public.gradebook USING btree (student_id);

CREATE INDEX idx_lesson_workload_id ON public.lesson USING btree (workload_id);

CREATE INDEX idx_parent_last_first_name ON public.parent USING btree (last_name, first_name);

CREATE INDEX idx_student_class_id ON public.student USING btree (class_id);

CREATE INDEX idx_student_last_first_name ON public.student USING btree (last_name, first_name);

CREATE INDEX idx_teacher_last_first_name ON public.teacher USING btree (last_name, first_name);

CREATE INDEX idx_workload_class_id ON public.workload USING btree (class_id);

CREATE INDEX idx_workload_teacher_id ON public.workload USING btree (teacher_id);

CREATE TRIGGER after_gradebook_change AFTER INSERT OR DELETE OR UPDATE ON public.gradebook FOR EACH ROW EXECUTE FUNCTION public.log_gradebook_changes();

CREATE TRIGGER before_grade_insert_update BEFORE INSERT OR UPDATE ON public.gradebook FOR EACH ROW EXECUTE FUNCTION public.check_grade_value();

CREATE TRIGGER trg_set_primary_school_flag BEFORE INSERT OR UPDATE ON public.class FOR EACH ROW EXECUTE FUNCTION public.update_primary_school_flag();

ALTER TABLE ONLY public.achievement
    ADD CONSTRAINT achievement_student_id_fkey FOREIGN KEY (student_id) REFERENCES public.student(id) ON DELETE CASCADE;

ALTER TABLE ONLY public.class
    ADD CONSTRAINT class_head_teacher_id_fkey FOREIGN KEY (head_teacher_id) REFERENCES public.teacher(id) ON DELETE SET NULL;

ALTER TABLE ONLY public.class
    ADD CONSTRAINT class_parallel_id_fkey FOREIGN KEY (parallel_id) REFERENCES public.parallel(id) ON DELETE RESTRICT;

ALTER TABLE ONLY public.gradebook
    ADD CONSTRAINT fk_gradebook_lesson FOREIGN KEY (lesson_id) REFERENCES public.lesson(id) ON DELETE CASCADE;

ALTER TABLE ONLY public.lesson
    ADD CONSTRAINT fk_lesson_workload FOREIGN KEY (workload_id) REFERENCES public.workload(id) ON DELETE CASCADE;

ALTER TABLE ONLY public.study_plan
    ADD CONSTRAINT fk_study_plan_academic_year FOREIGN KEY (academic_year_id) REFERENCES public.academic_year(id) ON DELETE RESTRICT;

ALTER TABLE ONLY public.workload
    ADD CONSTRAINT fk_workload_academic_year FOREIGN KEY (academic_year_id) REFERENCES public.academic_year(id) ON DELETE RESTRICT;

ALTER TABLE ONLY public.gradebook
    ADD CONSTRAINT gradebook_student_id_fkey FOREIGN KEY (student_id) REFERENCES public.student(id) ON DELETE CASCADE;

ALTER TABLE ONLY public.student
    ADD CONSTRAINT student_class_id_fkey FOREIGN KEY (class_id) REFERENCES public.class(id) ON DELETE RESTRICT;

ALTER TABLE ONLY public.student_parent
    ADD CONSTRAINT student_parent_parent_id_fkey FOREIGN KEY (parent_id) REFERENCES public.parent(id) ON DELETE CASCADE;

ALTER TABLE ONLY public.student_parent
    ADD CONSTRAINT student_parent_student_id_fkey FOREIGN KEY (student_id) REFERENCES public.student(id) ON DELETE CASCADE;

ALTER TABLE ONLY public.study_plan_item
    ADD CONSTRAINT study_plan_item_discipline_id_fkey FOREIGN KEY (discipline_id) REFERENCES public.discipline(id) ON DELETE RESTRICT;

ALTER TABLE ONLY public.study_plan_item
    ADD CONSTRAINT study_plan_item_study_plan_id_fkey FOREIGN KEY (study_plan_id) REFERENCES public.study_plan(id) ON DELETE CASCADE;

ALTER TABLE ONLY public.study_plan
    ADD CONSTRAINT study_plan_parallel_id_fkey FOREIGN KEY (parallel_id) REFERENCES public.parallel(id) ON DELETE CASCADE;

ALTER TABLE ONLY public.workload
    ADD CONSTRAINT workload_class_id_fkey FOREIGN KEY (class_id) REFERENCES public.class(id) ON DELETE RESTRICT;

ALTER TABLE ONLY public.workload
    ADD CONSTRAINT workload_discipline_id_fkey FOREIGN KEY (discipline_id) REFERENCES public.discipline(id) ON DELETE RESTRICT;

ALTER TABLE ONLY public.workload
    ADD CONSTRAINT workload_teacher_id_fkey FOREIGN KEY (teacher_id) REFERENCES public.teacher(id) ON DELETE RESTRICT;