using System;
using System.IO;
using System.Linq;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n===== SCHOOL SEARCH SYSTEM =====");
            Console.WriteLine("1 - Find student by last name");
            Console.WriteLine("2 - Find students by classroom");
            Console.WriteLine("3 - Find students by bus");
            Console.WriteLine("4 - Find teacher by name");
            Console.WriteLine("5 - Show students of a teacher");
            Console.WriteLine("6 - Add new student");
            Console.WriteLine("0 - Exit");

            Console.Write("Choose option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    FindStudentByLastName();
                    break;

                case "2":
                    FindStudentsByClassroom();
                    break;

                case "3":
                    FindStudentsByBus();
                    break;

                case "4":
                    FindTeacherByName();
                    break;

                case "5":
                    ShowStudentsOfTeacher();
                    break;

                case "6":
                    AddStudent();
                    break;

                case "0":
                    return;
            }
        }
    }

    static string FindTeacher(string classroom)
    {
        foreach (var line in File.ReadAllLines("teacher.txt"))
        {
            var parts = line.Split(',').Select(p => p.Trim()).ToArray();

            if (parts[2] == classroom)
                return parts[0] + " " + parts[1];
        }

        return "Unknown teacher";
    }

    static void FindStudentByLastName()
    {
        Console.Write("Enter student last name: ");
        string name = Console.ReadLine().ToUpper();

        var stopwatch = Stopwatch.StartNew();
        bool found = false;

        foreach (var line in File.ReadAllLines("list.txt"))
        {
            var parts = line.Split(',').Select(p => p.Trim()).ToArray();

            if (parts[0].ToUpper() == name)
            {
                string teacher = FindTeacher(parts[3]);

                Console.WriteLine($"\nStudent with last name {name} found:");
                Console.WriteLine($"Name: {parts[1]} {parts[0]}");
                Console.WriteLine($"Grade: {parts[2]}");
                Console.WriteLine($"Classroom: {parts[3]}");
                Console.WriteLine($"Bus: {parts[4]}");
                Console.WriteLine($"Teacher: {teacher}");
                found = true;
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");

        if (!found)
            Console.WriteLine("Student not found.");
    }

    static void FindStudentsByClassroom()
    {
        Console.Write("Enter classroom: ");
        string classroom = Console.ReadLine().Trim();

        var stopwatch = Stopwatch.StartNew();
        bool found = false;

        Console.WriteLine($"\nStudents in classroom {classroom}:");

        foreach (var line in File.ReadAllLines("list.txt"))
        {
            var parts = line.Split(',').Select(p => p.Trim()).ToArray();

            if (parts[3] == classroom)
            {
                Console.WriteLine($"{parts[1]} {parts[0]}");
                found = true;
            }
        }

        string teacher = FindTeacher(classroom);
        Console.WriteLine($"Teacher of classroom {classroom}: {teacher}");

        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");

        if (!found)
            Console.WriteLine("No students found in this classroom.");
    }

    static void FindStudentsByBus()
    {
        Console.Write("Enter bus number: ");
        string bus = Console.ReadLine().Trim();

        var stopwatch = Stopwatch.StartNew();
        bool found = false;

        Console.WriteLine($"\nStudents who ride bus {bus}:");

        foreach (var line in File.ReadAllLines("list.txt"))
        {
            var parts = line.Split(',').Select(p => p.Trim()).ToArray();

            if (parts[4] == bus)
            {
                Console.WriteLine($"{parts[1]} {parts[0]} (Classroom {parts[3]})");
                found = true;
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");

        if (!found)
            Console.WriteLine("No students found for this bus.");
    }

    static void FindTeacherByName()
    {
        Console.Write("Enter teacher last name: ");
        string name = Console.ReadLine().ToUpper();

        var stopwatch = Stopwatch.StartNew();
        bool found = false;

        foreach (var line in File.ReadAllLines("teacher.txt"))
        {
            var parts = line.Split(',').Select(p => p.Trim()).ToArray();

            if (parts[0].ToUpper() == name)
            {
                Console.WriteLine($"\nTeacher {parts[1]} {parts[0]} teaches classroom {parts[2]}.");
                found = true;
                break;
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");

        if (!found)
            Console.WriteLine("Teacher not found.");
    }

    static void ShowStudentsOfTeacher()
    {
        Console.Write("Enter teacher last name: ");
        string name = Console.ReadLine().ToUpper();

        var stopwatch = Stopwatch.StartNew();
        bool found = false;

        foreach (var teacherLine in File.ReadAllLines("teacher.txt"))
        {
            var t = teacherLine.Split(',').Select(p => p.Trim()).ToArray();

            if (t[0].ToUpper() == name)
            {
                string classroom = t[2];

                Console.WriteLine($"\nTeacher {t[1]} {t[0]} teaches the following students:");

                foreach (var studentLine in File.ReadAllLines("list.txt"))
                {
                    var s = studentLine.Split(',').Select(p => p.Trim()).ToArray();

                    if (s[3] == classroom)
                        Console.WriteLine($"{s[1]} {s[0]}");
                }

                found = true;
                break;
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");

        if (!found)
            Console.WriteLine("Teacher not found.");
    }

    static void AddStudent()
    {
        Console.WriteLine("\nAdding a new student:");

        Console.Write("Last Name: ");
        string last = Console.ReadLine().ToUpper().Trim();

        Console.Write("First Name: ");
        string first = Console.ReadLine().ToUpper().Trim();

        Console.Write("Grade: ");
        string grade = Console.ReadLine().Trim();

        Console.Write("Classroom: ");
        string classroom = Console.ReadLine().Trim();

        Console.Write("Bus: ");
        string bus = Console.ReadLine().Trim();

        string line = $"{last},{first},{grade},{classroom},{bus}";

        File.AppendAllText("list.txt", line + Environment.NewLine);

        Console.WriteLine("Student successfully added.");
    }
}