using AISchool.Models;
using Dapper;
using Npgsql;
using System.Configuration;
using System.Text.Json;

namespace AISchool.Data
{
    public class DataAccess : IDataAccess
    {
        private readonly string _connectionString;

        public DataAccess()
        {
            _connectionString = GetConnectionString();
        }

        private static string GetConnectionString(string name = "DefaultConnection")
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[name];
            if (connectionStringSettings == null)
            {
                throw new InvalidOperationException($"Строка подключения с именем '{name}' не найдена в файле конфигурации App.config или connectionStrings.config.");
            }
            return connectionStringSettings.ConnectionString;
        }

        public void AddBulkGrades(List<int> studentIds, int lessonId, int? grade, string workType, DateTime date)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_student_ids", studentIds);
                parameters.Add("p_lesson_id", lessonId);
                parameters.Add("p_grade", grade);
                parameters.Add("p_work_type", workType);
                parameters.Add("p_date", date, System.Data.DbType.Date);

                connection.Execute("SELECT add_bulk_grades(@p_student_ids, @p_lesson_id, @p_grade, @p_work_type, @p_date)", parameters);
            }
        }

        public void ClearBulkGrades(List<int> studentIds, int lessonId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT add_bulk_grades(@p_student_ids, @p_lesson_id, 0, '', NOW()::date)",
                   new { p_student_ids = studentIds, p_lesson_id = lessonId });
            }
        }

        public IEnumerable<AcademicPerformanceReportItem> GetAcademicPerformanceSummary(int academicYearId, DateTime startDate, DateTime endDate)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_academic_year_id", academicYearId);
                parameters.Add("p_start_date", startDate, System.Data.DbType.Date);
                parameters.Add("p_end_date", endDate, System.Data.DbType.Date);

                return connection.Query<AcademicPerformanceReportItem>(
                    "SELECT * FROM get_academic_performance_summary(@p_academic_year_id, @p_start_date, @p_end_date)",
                    parameters);
            }
        }

        public StudentMovementReport? GetStudentMovementReport(DateTime startDate, DateTime endDate)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_start_date", startDate, System.Data.DbType.Date);
                parameters.Add("p_end_date", endDate, System.Data.DbType.Date);

                string? jsonResult = connection.ExecuteScalar<string>(
                    "SELECT get_student_movement_report(@p_start_date, @p_end_date)",
                    parameters);

                if (string.IsNullOrEmpty(jsonResult))
                {
                    return null;
                }

                return JsonSerializer.Deserialize<StudentMovementReport>(jsonResult);
            }
        }

        public IEnumerable<TeacherWorkloadReportItem> GetTeacherWorkloadReport(int academicYearId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<TeacherWorkloadReportItem>(
                    "SELECT * FROM get_teacher_workload_report(@p_academic_year_id)",
                    new { p_academic_year_id = academicYearId });
            }
        }

        public IEnumerable<AcademicYear> GetAcademicYears()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<AcademicYear>("SELECT * FROM academic_year ORDER BY start_date DESC");
            }
        }

        public void CreateAcademicYear(string name, DateTime startDate, DateTime endDate)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO academic_year (name, start_date, end_date) VALUES (@Name, @StartDate, @EndDate)",
                    new { Name = name, StartDate = startDate, EndDate = endDate });
            }
        }

        public void UpdateAcademicYearStatus(int yearId, string status)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("UPDATE academic_year SET status = @Status WHERE id = @Id", new { Status = status, Id = yearId });
            }
        }

        public void ExecuteStudentPromotion(int completedYearId, int nextYearId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT promote_students_to_next_year(@p_completed_year_id, @p_next_year_id)",
                    new { p_completed_year_id = completedYearId, p_next_year_id = nextYearId });
            }
        }

        public IEnumerable<WorkloadView> GetWorkloadForClass(int classId, int academicYearId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<WorkloadView>("SELECT * FROM get_workload_for_class(@p_class_id, @p_academic_year_id)",
                    new { p_class_id = classId, p_academic_year_id = academicYearId });
            }
        }

        public void UpsertWorkload(int classId, int disciplineId, int teacherId, int academicYearId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT upsert_workload(@p_class_id, @p_discipline_id, @p_teacher_id, @p_academic_year_id)",
                    new { p_class_id = classId, p_discipline_id = disciplineId, p_teacher_id = teacherId, p_academic_year_id = academicYearId });
            }
        }

        public void DeleteWorkload(int classId, int disciplineId, int academicYearId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT delete_workload(@p_class_id, @p_discipline_id, @p_academic_year_id)",
                    new { p_class_id = classId, p_discipline_id = disciplineId, p_academic_year_id = academicYearId });
            }
        }

        public int AddStudyPlan(string name, int academicYearId, int parallelNumber)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("SELECT add_study_plan(@p_name, @p_academic_year_id, @p_parallel_number)",
                    new { p_name = name, p_academic_year_id = academicYearId, p_parallel_number = parallelNumber });
            }
        }

        public IEnumerable<StudyPlanView> GetAllStudyPlans()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<StudyPlanView>("SELECT * FROM get_all_study_plans()");
            }
        }

        public void ChangeUserPassword(int userId, string userRole, string newHash, string newSalt)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_user_id", userId);
                parameters.Add("p_user_role", userRole);
                parameters.Add("p_new_hash", Convert.FromBase64String(newHash));
                parameters.Add("p_new_salt", Convert.FromBase64String(newSalt));
                connection.Execute("SELECT change_user_password(@p_user_id, @p_user_role, @p_new_hash, @p_new_salt)", parameters);
            }
        }

        public IEnumerable<ClassDetails> GetAllClassesWithDetails()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<ClassDetails>("SELECT * FROM get_all_classes_with_details()");
            }
        }

        public int AddGradeToLesson(int studentId, int lessonId, int disciplineId, int? grade, DateTime date, string workType)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_student_id", studentId);
                parameters.Add("p_lesson_id", lessonId);
                parameters.Add("p_discipline_id", disciplineId);
                parameters.Add("p_grade", grade);
                parameters.Add("p_date", date, System.Data.DbType.Date);
                parameters.Add("p_work_type", workType);
                return connection.ExecuteScalar<int>(
                    "SELECT add_grade_to_lesson(@p_student_id, @p_lesson_id, @p_discipline_id, @p_grade, @p_date, @p_work_type)",
                    parameters);
            }
        }

        public IEnumerable<StudyPlanItem> GetStudyPlanItems(int studyPlanId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<StudyPlanItem>("SELECT * FROM get_study_plan_items(@p_study_plan_id)", new { p_study_plan_id = studyPlanId });
            }
        }

        public void UpsertStudyPlanItem(int studyPlanId, int disciplineId, int lessonsCount)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT upsert_study_plan_item(@p_study_plan_id, @p_discipline_id, @p_lessons_count)",
                    new { p_study_plan_id = studyPlanId, p_discipline_id = disciplineId, p_lessons_count = lessonsCount });
            }
        }

        public void DeleteStudyPlanItem(int itemId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT delete_study_plan_item(@p_item_id)", new { p_item_id = itemId });
            }
        }

        public class DisciplineInfo
        {
            public int DisciplineId { get; set; }
            public string DisciplineName { get; set; } = string.Empty;
        }

        public IEnumerable<DisciplineInfo> GetAllDisciplines()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<DisciplineInfo>("SELECT id as DisciplineId, name as DisciplineName FROM get_all_disciplines()");
            }
        }

        public int AddClass(string letter, int parallelNumber, int? headTeacherId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    return connection.ExecuteScalar<int>("SELECT add_class(@p_letter, @p_parallel_number, @p_head_teacher_id)",
                        new { p_letter = letter, p_parallel_number = parallelNumber, p_head_teacher_id = headTeacherId });
                }
                catch (PostgresException ex) when (ex.SqlState == "23505")
                {
                    if (ex.ConstraintName == "uq_class_head_teacher")
                    {
                        throw new InvalidOperationException("Ошибка: Этот преподаватель уже является классным руководителем другого класса.", ex);
                    }

                    if (ex.ConstraintName == "class_parallel_id_letter_key")
                    {
                        throw new InvalidOperationException($"Ошибка: Класс с буквой '{letter}' в {parallelNumber}-й параллели уже существует.", ex);
                    }

                    throw new InvalidOperationException("Ошибка уникальности данных при создании класса.", ex);
                }
                catch (NpgsqlException ex)
                {
                    throw new InvalidOperationException($"Ошибка базы данных: {ex.Message}", ex);
                }
            }
        }

        public void UpdateClassHeadTeacher(int classId, int? newHeadTeacherId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT update_class_head_teacher(@p_class_id, @p_new_head_teacher_id)",
                    new { p_class_id = classId, p_new_head_teacher_id = newHeadTeacherId });
            }
        }

        public IEnumerable<TeacherDetails> GetAllTeachers()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<TeacherDetails>("SELECT * FROM get_all_teachers()");
            }
        }

        public int AddTeacher(TeacherDetails teacher)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@LastName", teacher.LastName);
                parameters.Add("@FirstName", teacher.FirstName);
                parameters.Add("@Patronymic", teacher.Patronymic);
                parameters.Add("@Phone", teacher.Phone);
                parameters.Add("@Email", teacher.Email);
                parameters.Add("@Login", teacher.Login);
                parameters.Add("@PasswordHash", Convert.FromBase64String(teacher.PasswordHash!));
                parameters.Add("@PasswordSalt", Convert.FromBase64String(teacher.PasswordSalt!));
                parameters.Add("@Role", teacher.Role);
                parameters.Add("@Notes", teacher.Notes);
                parameters.Add("@AvatarImage", teacher.AvatarImage);
                return connection.ExecuteScalar<int>("SELECT add_teacher(@LastName, @FirstName, @Patronymic, @Phone, @Email, @Login, @PasswordHash, @PasswordSalt, @Role, @Notes, @AvatarImage)", parameters);
            }
        }

        public void UpdateTeacher(TeacherDetails teacher)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", teacher.Id);
                parameters.Add("@LastName", teacher.LastName);
                parameters.Add("@FirstName", teacher.FirstName);
                parameters.Add("@Patronymic", teacher.Patronymic);
                parameters.Add("@Phone", teacher.Phone);
                parameters.Add("@Email", teacher.Email);
                parameters.Add("@Login", teacher.Login);
                parameters.Add("@Role", teacher.Role);
                parameters.Add("@Notes", teacher.Notes);
                parameters.Add("@AvatarImage", teacher.AvatarImage);
                connection.Execute("SELECT update_teacher(@Id, @LastName, @FirstName, @Patronymic, @Phone, @Email, @Login, @Role, @Notes, @AvatarImage)", parameters);
            }
        }

        public void DeleteTeacher(int teacherId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    connection.Execute("SELECT delete_teacher(@p_id)", new { p_id = teacherId });
                }
                catch (NpgsqlException ex) when (ex.SqlState == "23503")
                {
                    throw new InvalidOperationException("Невозможно удалить учителя, так как за ним закреплена учебная нагрузка. Сначала снимите часы с преподавателя.", ex);
                }
            }
        }

        public AppUser? GetUserAuthDataByLogin(string login)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT 
                        t.id, 
                        concat_ws(' ', t.last_name, t.first_name, t.patronymic) as FullName, 
                        t.role, 
                        t.password_hash as PasswordHash, 
                        t.password_salt as PasswordSalt,
                        NULL::integer as RelatedStudentId,
                        t.avatar_image as AvatarImage
                    FROM teacher t WHERE t.login = @Login
                    UNION ALL
                    SELECT 
                        s.id, 
                        concat_ws(' ', s.last_name, s.first_name, s.patronymic), 
                        'student', 
                        s.password_hash, 
                        s.password_salt,
                        s.id,
                        s.avatar_image
                    FROM student s WHERE s.login = @Login
                    UNION ALL
                    SELECT 
                        p.id, 
                        concat_ws(' ', p.last_name, p.first_name, p.patronymic), 
                        'parent', 
                        p.password_hash, 
                        p.password_salt,
                        (SELECT sp.student_id FROM student_parent sp WHERE sp.parent_id = p.id LIMIT 1),
                        p.avatar_image
                    FROM parent p WHERE p.login = @Login";

                return connection.QueryFirstOrDefault<AppUser>(sql, new { Login = login });
            }
        }

        public void UpdateUserAvatar(int userId, string role, byte[]? avatarImage)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT update_user_avatar(@p_user_id, @p_role, @p_image)",
                    new { p_user_id = userId, p_role = role, p_image = avatarImage });
            }
        }

        public IEnumerable<DisciplineInfo> GetDisciplinesForStudent(int studentId, int teacherId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM get_disciplines_for_student(@p_student_id, @p_teacher_id)";
                return connection.Query<DisciplineInfo>(sql, new { p_student_id = studentId, p_teacher_id = teacherId });
            }
        }

        public IEnumerable<DisciplineInfo> GetDisciplinesForStudent(int studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM get_disciplines_for_student(@p_student_id)";
                return connection.Query<DisciplineInfo>(sql, new { p_student_id = studentId });
            }
        }

        public IEnumerable<LessonInfo> GetStudentLessonsAndGrades(int studentId, int disciplineId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<LessonInfo>(
                    "SELECT * FROM get_student_lessons_and_grades(@p_student_id, @p_discipline_id)",
                    new { p_student_id = studentId, p_discipline_id = disciplineId });
            }
        }

        public void UpdateGrade(int gradebookId, int newGrade, string newWorkType)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute(
                    "SELECT update_grade(@p_gradebook_id, @p_new_grade, @p_new_work_type)",
                    new { p_gradebook_id = gradebookId, p_new_grade = newGrade, p_new_work_type = newWorkType }
                );
            }
        }

        public AcademicYear? GetCurrentAcademicYear()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<AcademicYear>("SELECT * FROM academic_year WHERE status = 'Current' LIMIT 1");
            }
        }

        public IEnumerable<DisciplineInfo> GetDisciplinesForClass(int classId, int teacherId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<DisciplineInfo>(
                    @"SELECT DISTINCT d.id as DisciplineId, d.name as DisciplineName
                      FROM workload w
                      JOIN discipline d ON w.discipline_id = d.id
                      WHERE w.class_id = @classId AND w.teacher_id = @teacherId
                      ORDER BY d.name",
                    new { classId, teacherId });
            }
        }

        public JournalDataSource GetJournalDataSource(int classId, int disciplineId, int academicYearId, DateTime startDate, DateTime endDate)
        {
            var result = new JournalDataSource();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var lessonParams = new DynamicParameters();
                lessonParams.Add("p_class_id", classId);
                lessonParams.Add("p_discipline_id", disciplineId);
                lessonParams.Add("p_academic_year_id", academicYearId);
                lessonParams.Add("p_start_date", startDate, System.Data.DbType.Date);
                lessonParams.Add("p_end_date", endDate, System.Data.DbType.Date);

                using (var multi = connection.QueryMultiple(
                    @"SELECT * FROM get_journal_students(@p_class_id);
                      SELECT * FROM get_journal_lessons(@p_class_id, @p_discipline_id, 
                             @p_academic_year_id, @p_start_date, @p_end_date);
                      SELECT * FROM get_journal_grades(@p_class_id, @p_discipline_id, 
                             @p_academic_year_id);",
                    lessonParams))
                {
                    result.Students = multi.Read<JournalStudent>().ToList();
                    result.Lessons = multi.Read<JournalLesson>().ToList();
                    result.Grades = multi.Read<JournalGrade>().ToList();
                }
            }
            return result;
        }

        public IEnumerable<ClassInfo> GetClassesForTeacher(int teacherId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM get_all_classes_for_teacher(@p_teacher_id)";
                return connection.Query<ClassInfo>(sql, new { p_teacher_id = teacherId });
            }
        }

        public IEnumerable<StudentInfo> GetStudentsInClass(int classId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = "SELECT id, last_name, first_name, patronymic FROM student WHERE status = 'Active' AND class_id = @ClassId ORDER BY last_name, first_name";
                return connection.Query<StudentInfo>(sql, new { ClassId = classId });
            }
        }

        public StudentProfile? GetStudentProfile(int studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT 
                        s.id, s.last_name, s.first_name, s.patronymic, s.birth_date, s.notes, s.avatar_image,
                        p.number || ' ""' || c.letter || '""' AS class_name,
                        s.class_id 
                    FROM student s
                    JOIN class c ON s.class_id = c.id
                    JOIN parallel p ON c.parallel_id = p.id
                    WHERE s.id = @StudentId;";
                return connection.QueryFirstOrDefault<StudentProfile>(sql, new { StudentId = studentId });
            }
        }

        public IEnumerable<AverageGrade> GetStudentAverageGrades(int studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM get_student_average_grades(@p_student_id)";
                return connection.Query<AverageGrade>(sql, new { p_student_id = studentId });
            }
        }

        public IEnumerable<Achievement> GetStudentAchievements(int studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = "SELECT id, event_date, event_name, level, place FROM achievement WHERE student_id = @StudentId ORDER BY event_date DESC";
                return connection.Query<Achievement>(sql, new { StudentId = studentId });
            }
        }

        public void DeleteGrade(int gradebookId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT delete_grade(@p_gradebook_id)", new { p_gradebook_id = gradebookId });
            }
        }

        public int AddAchievement(int studentId, Achievement achievement)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_student_id", studentId);
                parameters.Add("p_event_name", achievement.EventName);
                parameters.Add("p_event_date", achievement.EventDate);
                parameters.Add("p_level", achievement.Level);
                parameters.Add("p_place", achievement.Place);
                return connection.ExecuteScalar<int>("SELECT add_achievement(@p_student_id, @p_event_name, @p_event_date, @p_level, @p_place)", parameters);
            }
        }

        public void UpdateAchievement(Achievement achievement)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_id", achievement.Id);
                parameters.Add("p_event_name", achievement.EventName);
                parameters.Add("p_event_date", achievement.EventDate);
                parameters.Add("p_level", achievement.Level);
                parameters.Add("p_place", achievement.Place);
                connection.Execute("SELECT update_achievement(@p_id, @p_event_name, @p_event_date, @p_level, @p_place)", parameters);
            }
        }

        public void DeleteAchievement(int achievementId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT delete_achievement(@p_id)", new { p_id = achievementId });
            }
        }

        public void UpdateLessonDetails(int lessonId, DateTime? lessonDate, string topic)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new Dapper.DynamicParameters();
                parameters.Add("p_lesson_id", lessonId);
                parameters.Add("p_lesson_date", lessonDate, System.Data.DbType.Date);
                parameters.Add("p_topic", topic);

                connection.Execute("SELECT update_lesson_details(@p_lesson_id, @p_lesson_date, @p_topic)", parameters);
            }
        }

        public IEnumerable<GradeEntry> GetGradesForStudentLesson(int lessonId, int studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.Query<GradeEntry>(
                    "SELECT * FROM get_grades_for_student_lesson(@p_lesson_id, @p_student_id)",
                    new { p_lesson_id = lessonId, p_student_id = studentId });
            }
        }

        public int AddStudent(StudentDetails student)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@LastName", student.LastName);
                parameters.Add("@FirstName", student.FirstName);
                parameters.Add("@Patronymic", student.Patronymic);
                parameters.Add("@ClassId", student.ClassId);
                parameters.Add("@BirthDate", student.BirthDate, System.Data.DbType.Date);
                parameters.Add("@Notes", student.Notes);
                parameters.Add("@AvatarImage", student.AvatarImage);

                parameters.Add("@Login", student.Login);

                if (!string.IsNullOrEmpty(student.Password))
                {
                    var (hash, salt) = PasswordHasher.HashPassword(student.Password);
                    parameters.Add("@PasswordHash", Convert.FromBase64String(hash));
                    parameters.Add("@PasswordSalt", Convert.FromBase64String(salt));
                }
                else
                {
                    parameters.Add("@PasswordHash", null);
                    parameters.Add("@PasswordSalt", null);
                }

                return connection.ExecuteScalar<int>("SELECT add_student(@LastName, @FirstName, @Patronymic, @ClassId, @BirthDate, @Notes, @AvatarImage, @Login, @PasswordHash, @PasswordSalt)", parameters);
            }
        }

        public void UpdateStudent(StudentDetails student)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", student.Id);
                parameters.Add("@LastName", student.LastName);
                parameters.Add("@FirstName", student.FirstName);
                parameters.Add("@Patronymic", student.Patronymic);
                parameters.Add("@ClassId", student.ClassId);
                parameters.Add("@BirthDate", student.BirthDate, System.Data.DbType.Date);
                parameters.Add("@Notes", student.Notes);
                parameters.Add("@AvatarImage", student.AvatarImage);
                parameters.Add("@Login", student.Login);

                connection.Execute("SELECT update_student(@Id, @LastName, @FirstName, @Patronymic, @ClassId, @BirthDate, @Notes, @AvatarImage, @Login)", parameters);
            }
        }

        public void ExpelStudent(int studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT expel_student(@p_student_id)", new { p_student_id = studentId });
            }
        }

        public StudentDetails? GetStudentDetails(int studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<StudentDetails>(
                    "SELECT id, last_name, first_name, patronymic, class_id, birth_date, notes, avatar_image as AvatarImage, login FROM student WHERE id = @Id",
                    new { Id = studentId });
            }
        }

        public void TransferStudent(int studentId, int newClassId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute("SELECT transfer_student(@p_student_id, @p_new_class_id)",
                    new { p_student_id = studentId, p_new_class_id = newClassId });
            }
        }

        public IEnumerable<ParentInfo> GetParentsForStudent(int studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT 
                        p.id, 
                        p.last_name, 
                        p.first_name, 
                        p.patronymic, 
                        p.phone, 
                        p.email,
                        p.login, 
                        p.avatar_image as AvatarImage
                    FROM parent p
                    JOIN student_parent sp ON p.id = sp.parent_id
                    WHERE sp.student_id = @p_student_id
                    ORDER BY p.last_name, p.first_name";
                return connection.Query<ParentInfo>(sql, new { p_student_id = studentId });
            }
        }

        public IEnumerable<ParentInfo> GetAllParentsForLinking(int studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT 
                        p.id, 
                        p.last_name, 
                        p.first_name, 
                        p.patronymic, 
                        p.phone, 
                        p.email,
                        p.login, 
                        p.avatar_image as AvatarImage
                    FROM parent p
                    WHERE NOT EXISTS (
                        SELECT 1 FROM student_parent sp
                        WHERE sp.parent_id = p.id AND sp.student_id = @p_student_id
                    )
                    ORDER BY p.last_name, p.first_name";
                return connection.Query<ParentInfo>(sql, new { p_student_id = studentId });
            }
        }

        public int AddParent(ParentInfo parent)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>(
                    "SELECT add_parent(@LastName, @FirstName, @Patronymic, @Phone, @Email, @Login, @PasswordHash, @PasswordSalt, @AvatarImage)",
                    new
                    {
                        parent.LastName,
                        parent.FirstName,
                        parent.Patronymic,
                        parent.Phone,
                        parent.Email,
                        parent.Login,
                        PasswordHash = Convert.FromBase64String(parent.PasswordHash!),
                        PasswordSalt = Convert.FromBase64String(parent.PasswordSalt!),
                        parent.AvatarImage
                    });
            }
        }

        public void UpdateParent(ParentInfo parent)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute(
                    "SELECT update_parent(@Id, @LastName, @FirstName, @Patronymic, @Phone, @Email, @Login, @AvatarImage)",
                    parent);
            }
        }

        public void LinkStudentToParent(int studentId, int parentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute(
                    "SELECT link_student_to_parent(@p_student_id, @p_parent_id)",
                    new { p_student_id = studentId, p_parent_id = parentId });
            }
        }

        public void UnlinkStudentFromParent(int studentId, int parentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute(
                    "SELECT unlink_student_from_parent(@p_student_id, @p_parent_id)",
                    new { p_student_id = studentId, p_parent_id = parentId });
            }
        }
    }
}