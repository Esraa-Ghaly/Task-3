using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementSystem
{
    // ===== Student Class =====
    class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();

        public bool Enroll(Course course)
        {
            if (!Courses.Contains(course))
            {
                Courses.Add(course);
                return true;
            }
            return false;
        }

        public string PrintDetails()
        {
            string courseTitles = Courses.Count == 0 ? "None" : string.Join(", ", Courses.Select(c => c.Title));
            return $"ID: {StudentId}, Name: {Name}, Age: {Age}, Courses: {courseTitles}";
        }
    }

    // ===== Instructor Class =====
    class Instructor
    {
        public int InstructorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }

        public string PrintDetails()
        {
            return $"Instructor ID: {InstructorId}, Name: {Name}, Specialization: {Specialization}";
        }
    }

    // ===== Course Class =====
    class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public Instructor Instructor { get; set; }

        public string PrintDetails()
        {
            return $"Course ID: {CourseId}, Title: {Title}, Instructor: {Instructor?.Name ?? "None"}";
        }
    }

    // ===== StudentManager Class =====
    class StudentManager
    {
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();

        public bool AddStudent(Student student)
        {
            if (FindStudent(student.StudentId) != null) return false;
            Students.Add(student);
            return true;
        }

        public bool AddCourse(Course course)
        {
            if (FindCourse(course.CourseId) != null) return false;
            Courses.Add(course);
            return true;
        }

        public bool AddInstructor(Instructor instructor)
        {
            if (FindInstructor(instructor.InstructorId) != null) return false;
            Instructors.Add(instructor);
            return true;
        }

        public Student FindStudent(int studentId) => Students.FirstOrDefault(s => s.StudentId == studentId);
        public Student FindStudent(string name) => Students.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        course FindCourse(int courseId) => Courses.FirstOrDefault(c => c.CourseId == courseId);
        course FindCourse(string name) => Courses.FirstOrDefault(c => c.Title.Equals(name, StringComparison.OrdinalIgnoreCase));

        public Instructor FindInstructor(int instructorId) => Instructors.FirstOrDefault(i => i.InstructorId == instructorId);

        public bool EnrollStudentInCourse(int studentId, int courseId)
        {
            Student student = FindStudent(studentId);
            Course course = FindCourse(courseId);
            if (student == null || course == null) return false;
            return student.Enroll(course);
        }

        public bool IsStudentEnrolledInCourse(int studentId, int courseId)
        {
            var student = FindStudent(studentId);
            var course = FindCourse(courseId);
            return student != null && course != null && student.Courses.Contains(course);
        }

        public string GetInstructorByCourseName(string courseName)
        {
            var course = FindCourse(courseName);
            return course?.Instructor?.Name ?? "Instructor not found.";
        }
    }

    // ===== Program Entry Point =====
    class Program
    {
        static StudentManager manager = new StudentManager();

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n=== Student Management System ===");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Add Instructor");
                Console.WriteLine("3. Add Course");
                Console.WriteLine("4. Enroll Student in Course");
                Console.WriteLine("5. Show All Students");
                Console.WriteLine("6. Show All Courses");
                Console.WriteLine("7. Show All Instructors");
                Console.WriteLine("8. Find Student by ID or Name");
                Console.WriteLine("9. Find Course by ID or Name");
                Console.WriteLine("10. Exit");
                Console.WriteLine("11. Check if Student is Enrolled in Course");
                Console.WriteLine("12. Get Instructor by Course Name");
                Console.Write("Select option: ");

                string input = Console.ReadLine();
                Console.WriteLine();
                switch (input)
                {
                    case "1": AddStudent(); break;
                    case "2": AddInstructor(); break;
                    case "3": AddCourse(); break;
                    case "4": EnrollStudent(); break;
                    case "5": ShowStudents(); break;
                    case "6": ShowCourses(); break;
                    case "7": ShowInstructors(); break;
                    case "8": FindStudent(); break;
                    case "9": FindCourse(); break;
                    case "10": return;
                    case "11": CheckEnrollment(); break;
                    case "12": GetInstructorByCourseName(); break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        static void AddStudent()
        {
            Console.Write("ID: "); int id = int.Parse(Console.ReadLine());
            Console.Write("Name: "); string name = Console.ReadLine();
            Console.Write("Age: "); int age = int.Parse(Console.ReadLine());

            if (manager.AddStudent(new Student { StudentId = id, Name = name, Age = age }))
                Console.WriteLine("Student added.");
            else
                Console.WriteLine("Student with this ID already exists.");
        }

        static void AddInstructor()
        {
            Console.Write("ID: "); int id = int.Parse(Console.ReadLine());
            Console.Write("Name: "); string name = Console.ReadLine();
            Console.Write("Specialization: "); string spec = Console.ReadLine();

            if (manager.AddInstructor(new Instructor { InstructorId = id, Name = name, Specialization = spec }))
                Console.WriteLine("Instructor added.");
            else
                Console.WriteLine("Instructor already exists.");
        }

        static void AddCourse()
        {
            Console.Write("Course ID: "); int id = int.Parse(Console.ReadLine());
            Console.Write("Title: "); string title = Console.ReadLine();
            Console.Write("Instructor ID: "); int instId = int.Parse(Console.ReadLine());

            Instructor instructor = manager.FindInstructor(instId);
            if (instructor == null)
            {
                Console.WriteLine("Instructor not found.");
                return;
            }

            if (manager.AddCourse(new Course { CourseId = id, Title = title, Instructor = instructor }))
                Console.WriteLine("Course added.");
            else
                Console.WriteLine("Course already exists.");
        }

        static void EnrollStudent()
        {
            Console.Write("Student ID: "); int sid = int.Parse(Console.ReadLine());
            Console.Write("Course ID: "); int cid = int.Parse(Console.ReadLine());

            if (manager.EnrollStudentInCourse(sid, cid))
                Console.WriteLine("Student enrolled.");
            else
                Console.WriteLine("Enrollment failed.");
        }

        static void ShowStudents()
        {
            foreach (var s in manager.Students)
                Console.WriteLine(s.PrintDetails());
        }

        static void ShowCourses()
        {
            foreach (var c in manager.Courses)
                Console.WriteLine(c.PrintDetails());
        }

        static void ShowInstructors()
        {
            foreach (var i in manager.Instructors)
                Console.WriteLine(i.PrintDetails());
        }

        static void FindStudent()
        {
            Console.Write("Search by (id/name): ");
            string input = Console.ReadLine();
            Student s = null;
            if (int.TryParse(input, out int id))
                s = manager.FindStudent(id);
            else
                s = manager.FindStudent(input);

            Console.WriteLine(s?.PrintDetails() ?? "Student not found.");
        }

        static void FindCourse()
        {
            Console.Write("Search by (id/name): ");
            string input = Console.ReadLine();
            Course c = null;
            if (int.TryParse(input, out int id))
                c = manager.FindCourse(id);
            else
                c = manager.FindCourse(input);

            Console.WriteLine(c?.PrintDetails() ?? "Course not found.");
        }

        static void CheckEnrollment()
        {
            Console.Write("Student ID: ");
            int sid = int.Parse(Console.ReadLine());
            Console.Write("Course ID: ");
            int cid = int.Parse(Console.ReadLine());

            Console.WriteLine(manager.IsStudentEnrolledInCourse(sid, cid)
                ? "Student is enrolled in the course."
                : "Student is not enrolled in the course.");
        }

        static void GetInstructorByCourseName()
        {
            Console.Write("Course Name: ");
            string courseName = Console.ReadLine();
            Console.WriteLine(manager.GetInstructorByCourseName(courseName));
        }
    }
}
