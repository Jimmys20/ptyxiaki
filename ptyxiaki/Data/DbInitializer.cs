using ptyxiaki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ptyxiaki.Data
{
  public static class DbInitializer
  {
    public static void Initialize(DepartmentContext context)
    {
      context.Database.EnsureDeleted();
      context.Database.EnsureCreated();

      if (context.professors.Any())
      {
        return;
      }

      context.programsOfStudies.Add(new ProgramOfStudies { title = "ΠΣ1" });
      context.SaveChanges();

      insertProfessors(context);
      insertCourses(context);

      Semester semester = new Semester { title = "Χ1819", createdAt = new DateTime(2018, 1, 1) };
      context.semesters.Add(semester);
      for(int i = 0;i<10;i++)
      {
        context.semesters.Add(new Semester { title = "sem" + i, createdAt = DateTime.Now });
      }

     
      context.SaveChanges();

      var students = new List<Student>
      {
        new Student{ studentId=-134007, firstName="Δημήτρης", lastName="Μαραγκός", registrationNumber="134007", semester=3,average=100,credits=300, progressFactor=120 },
        new Student{ studentId=-2, firstName="Ελισάβετ", lastName="Αναστασιάδου", registrationNumber="144087", semester=7 },
        new Student{ studentId=-3, firstName="Γιώργος", lastName="Μαραγκός", registrationNumber="194207", semester=5 },
        new Student{ studentId=-4, firstName="Λιάνα", lastName="Πιπεροπούλου", registrationNumber="114307", semester=3 },
        new Student{ studentId=-5, firstName="Παναγιώτης", lastName="Πιπερόπουλος", registrationNumber="124034", semester=1 },
        new Student{ studentId=-6, firstName="Σίμος", lastName="Μαραγκός", registrationNumber="104107", semester=3 },
        new Student{ studentId=-7, firstName="Λάκης", lastName="Λαλάκης", registrationNumber="155123", semester=5 }
      };
      //for (var i = 10; i < 11; i++)
      //{
      //  students.Add(new Student { studentId = -1 * i, firstName = "Student" + i, lastName = "lastname" + i, registrationNumber = i * 10000 });
      //}
      foreach (Student s in students)
      {
        //s.grades = new List<Grade>
        //{
        // // new Grade{courseId=5 }
        //  //new Grade{courseId=10 },
        //  //new Grade{courseId=4 },
        //  //new Grade{courseId=9 }
        //};
        context.students.Add(s);
      }
      context.SaveChanges();

      var thesisTypes = new Category[]
      {
        new Category { title = "Ανάπτυξη Διαδικτυακής Εφαρμογής", description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in " },
        new Category { title = "Βιβλιογραφική Επισκόπηση", description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making" },
        new Category { title = "Δημιουργία Εκπαιδευτικού Περιεχομένου" },
        new Category { title = "Έρευνα" },
        new Category { title = "Εφαρμογές Λογισμικού" },
        new Category { title = "Νέες Τεχνολογίες" },
        new Category { title = "Πιλοτική Εφαρμογή" }
      };
      foreach (Category c in thesisTypes)
      {
        context.categories.Add(c);
      }
      context.SaveChanges();

      //var assignments = new Assignment[]
      //{
      //  new Assignment{ studentId = -134007, thesisId=-5 },
      //  new Assignment{ studentId = -134007, thesisId=-2 },
      //  new Assignment{ studentId = -134007, thesisId=-6 }
      //};
      //foreach (Assignment c in assignments)
      //{
      //  context.assignments.Add(c);
      //}
      //context.SaveChanges();


      Thesis thesis = new Thesis
      {
        assignedAt = new DateTime(2010, 1, 1),
        semesterId = 1,
        title = "title",
        englishTitle = "englishTitle",
        description = "description",
        professorId = 4,
        status = Status.Active,
        assignments = new List<Assignment>
        {
          new Assignment
          {
            studentId = -2
          },
          new Assignment
          {
            studentId = -3
          }
        }
      };

      context.theses.Add(thesis);
      context.SaveChanges();

      Thesis thesis2 = new Thesis
      {
        semesterId = 1,
        assignedAt = new DateTime(2011, 1, 1),
        status = Status.Active,
        title = "bbb",
        englishTitle = "aaa",
        description = "descr",
        professorId = 2,
        assignments = new List<Assignment>
        {
          new Assignment
          {
            studentId = -4
          },
          new Assignment
          {
            studentId = -5
          }
        }
      };

      context.theses.Add(thesis2);
      context.SaveChanges();

      Thesis thesis3 = context.theses.Find(2);
      thesis3.semesterId = 1;
      thesis3.assignedAt = new DateTime(2012, 1, 1);
      thesis3.assignments = new List<Assignment>
      {
        new Assignment
        {
          studentId = -6
        }
      };

      context.SaveChanges();

      

      var theses = new Thesis[]
      {
        new Thesis
        {
          thesisId = -1,
          professorId = 1,
          title = "ACTIVE",
          description = "ΛΑΛΑΛΑΛΑ",
          englishTitle = "java",
          status = Status.Active,
          semesterId = 1,
          createdAt = new DateTime(2019,1,2),
          assignedAt = new DateTime(2019,2,3),
          assignments = new List<Assignment>
          {
            //new Assignment { studentId = -134007 }
          },
          categorizations = new List<Categorization>
          {
            new Categorization { categoryId = 1 },
            new Categorization { categoryId = 3 }
          },
          requirements = new List<Requirement>
          {
            new Requirement { courseId = 5 },
            new Requirement { courseId = 10 }
          }
        },
        new Thesis
        {
          thesisId = -2,
          professorId = 7,
          title = "Canceled",
          description = "",
          englishTitle = "java",
          status = Status.Canceled,
          semesterId = 1,
          createdAt = new DateTime(2019,1,2),
          assignedAt = new DateTime(2018,2,3),
          assignments = new List<Assignment>
          {
            //new Assignment { studentId = -134007 }
          },
          categorizations = new List<Categorization>
          {
            new Categorization { categoryId = 1 },
            new Categorization { categoryId = 3 }
          },
          requirements = new List<Requirement>
          {
            new Requirement { courseId = 5 },
            new Requirement { courseId = 10 }
          }
        },
        new Thesis
        {
          thesisId = -3,
          professorId = 3,
          title = "Completed",
          description = "",
          englishTitle = "java",
          status = Status.Completed,
          semesterId = 1,
          createdAt = new DateTime(2019,1,2),
          assignedAt = new DateTime(2017,2,3),
          completedAt = new DateTime(2019,8,6),
          assignments = new List<Assignment>
          {
            //new Assignment { studentId = -134007 }
          },
          categorizations = new List<Categorization>
          {
            new Categorization { categoryId = 2 },
            new Categorization { categoryId = 4 }
          },
          requirements = new List<Requirement>
          {
            new Requirement { courseId = 4 },
            new Requirement { courseId = 9 }
          }
        },
        new Thesis
        {
          thesisId = -4,
          professorId = 7,
          title = "c# lallalala mpapapa kkkk ola opa terma kka",
          description = "Available",
          englishTitle = "java",
          status = Status.Available,
          semesterId = 1,
          createdAt = new DateTime(2019,1,2),
          assignedAt = new DateTime(2010, 1, 1),
          categorizations = new List<Categorization>
          {
            new Categorization { categoryId = 1 },
            new Categorization { categoryId = 3 }
          },
          requirements = new List<Requirement>
          {
            new Requirement { courseId = 5 },
            new Requirement { courseId = 10 }
          }
        },
        new Thesis
        {
          semesterId = 1,
          thesisId = -5,
          professorId = 4,
          title = "whatever",
          description = "",
          englishTitle = "java",
          status = Status.Active
        },
        new Thesis
        {
          semesterId = 1,
          thesisId = -6,
          professorId = 5,
          title = "titlos",
          description = "",
          englishTitle = "java",
          assignedAt = new DateTime(2012, 1, 1),
          assignments = new List<Assignment>
          {
            //new Assignment { studentId = -134007 }
          },
          status = Status.Canceled
        },
        new Thesis
        {
          semesterId = 1,
          thesisId = -7,
          professorId = 7,
          title = "titlosss",
          description = "",
          englishTitle = "java",
          status = Status.Completed
        }
      };
      foreach (Thesis t in theses)
      {
        context.theses.Add(t);
      }
      context.SaveChanges();

      context.declarations.Add(new Declaration
      {
        //priority = 1,
        studentId = -2,
        thesisId = -5,
        semesterId = 1
      });

      context.declarations.Add(new Declaration
      {
        //priority = 2,
        studentId = -3,
        //studentId2 = -7,
        thesisId = -5,
        semesterId = 1
      });

      context.declarations.Add(new Declaration
      {
    //    priority = 3,
        studentId = -4,
        //studentId2 = -2,
        thesisId = -5,
        semesterId = 1
      });

    //  context.declarations.Add(new Declaration
    //  {
    ////    priority = 4,
    //    studentId = -134007,
    //    //studentId2 = -3,
    //    thesisId = -7,
    //    semesterId = 1
    //  });

    //  context.declarations.Add(new Declaration
    //  {
    //   // priority = 5,
    //    studentId = -134007,
    //   // studentId2 = -4,
    //    thesisId = -7,
    //    semesterId = 1
    //  });

    //  context.declarations.Add(new Declaration
    //  {
    //    //priority = 6,
    //    studentId = -134007,
    //    thesisId = -7,
    //    semesterId = 1
    //  });

    //  context.declarations.Add(new Declaration
    //  {
    //  //  priority = 7,
    //    studentId = -134007,
    //    thesisId = -7,
    //    semesterId = 1
    //  });

      context.declarations.Add(new Declaration
      {
    //    priority = 8,
        studentId = -134007,
        thesisId = -7,
        semesterId = 1
      });

      context.SaveChanges();

      //for (var i = 10000; i < 20000; i++)
      //{
      //  context.students.Add(new Student
      //  {
      //    studentId = i,
      //    firstName = $"{i}firstName",
      //    lastName = $"{i}lastName"
      //  });
      //}

      //context.SaveChanges();

      for (var i = 0; i < 2000; i++)
      {
        context.theses.Add(new Thesis
        {
          semesterId = 1,
          title = "" + i,
          englishTitle = "" + i,
          status = Status.Active,
          description = "descr",
          professorId = 7
        });
      }

      for (var i = 0; i < 250; i++)
      {
        context.theses.Add(new Thesis
        {
          semesterId = 1,
          title = "" + i,
          englishTitle = "" + i,
          status = Status.Available,
          description = "descr",
          professorId = 7
        });
      }

      for (var i = 0; i < 250; i++)
      {
        context.theses.Add(new Thesis
        {
          semesterId = 1,
          title = "" + i,
          englishTitle = "" + i,
          status = Status.Canceled,
          description = "descr",
          professorId = 7
        });
      }

      for (var i = 0; i < 250; i++)
      {
        context.theses.Add(new Thesis
        {
          semesterId = 1,
          title = "" + i,
          englishTitle = "" + i,
          status = Status.Completed,
          description = "descr",
          professorId = 7
        });
      }

      context.SaveChanges();
    }

    private static void insertCourses(DbContext context)
    {
      context.Database.ExecuteSqlCommand(
        @"INSERT INTO courses (""title"", ""semester"", ""code"", ""programOfStudiesId"") VALUES
('Αλγοριθμική και Προγραμματισμός', 1, '5102', 1),
('Αλληλεπίδραση Ανθρώπου-Μηχανής και Ανάπτυξη διεπιφανειών Χρήστη', 3, '5304', 1),
('Αναγνώριση Προτύπων - Νευρωνικά Δίκτυα', 6, '5611', 1),
('Ανάπτυξη Διαδ. Συστημ. και Εφαρμογών', 5, '5504', 1),
('Ανάπτυξη και Διαχείριση Ολοκληρωμένων Πλ. Συστημάτων και Εφαρμογών', 7, '5701', 1),
('Αντικειμενοστρεφής Προγραμματισμός', 2, '5201', 1),
('Αριθ. Ανάλυση και Προγ/μος Επιστημονικών Εφαρμογών', 3, '5301', 1),
('Αρχές Σχεδίασης Λειτουργικών Συστημάτων', 5, '5501', 1),
('Ασφάλεια Πληροφορικών Συστημάτων', 6, '5606', 1),
('Γλώσσες και Τεχνολογίες Ιστού', 2, '5204', 1),
('Γραφικά Υπολογιστών', 7, '5703', 1),
('Δεξιότητες Επικοινωνίας/Κοινωνικά Δίκτυα', 1, '5105', 1),
('Διαδικτυακές Υπηρεσίες Προστιθέμενης Αξίας (e-com/gov/e-learn)', 7, '5706', 1),
('Διαδίκτυο των Πραγμάτων', 6, '5613', 1),
('Διακριτά Μαθηματικά', 2, '5203', 1),
('Διαχείριση Συστήματος και Υπηρεσιών DBMS', 7, '5711', 1),
('Δίκτυα Ασύρματων και Κινητών Επικοινωνιών', 6, '5608', 1),
('Δίκτυα Η/Υ', 5, '5503', 1),
('Δίκτυα Καθοριζόμενα από Λογισμικό', 6, '5612', 1),
('Διοίκηση και Διαχείριση Έργων Πληροφορικής', 6, '5610', 1),
('Δομές Δεδομένων και Ανάλυση Αλγορίθμων', 3, '5302', 1),
('Ειδικά θέματα δικτύων I', 7, '5705', 1),
('Ειδικά θέματα δικτύων II', 6, '5607', 1),
('Εισαγωγή στα Λειτουργικά Συστήματα', 2, '5202', 1),
('Εισαγωγή στην Πληροφορική', 1, '5101', 1),
('Επιχειρησιακή Έρευνα', 5, '5505', 1),
('Ευφυή Συστήματα', 6, '5605', 1),
('Θεωρία Πιθανοτήτων και Στατιστική', 4, '5405', 1),
('Μαθηματική Ανάλυση και Γραμμική Άλγεβρα', 1, '5104', 1),
('Μεθοδολογίες Προγραμματισμού', 4, '5403', 1),
('Μηχανική Λογισμικού IΙ', 6, '5602', 1),
('Μηχανική Λογισμικού Ι', 5, '5502', 1),
('Μηχανική Μάθηση', 6, '5604', 1),
('Οργάνωση Δεδομένων και Εξόρυξη Πληροφορίας', 6, '5603', 1),
('Οργάνωση και Αρχιτεκτονική Υπολ. Συστημάτων', 3, '5303', 1),
('Πληροφοριακά Συστήματα Ι', 2, '5205', 1),
('Πληροφοριακά Συστήματα ΙΙ', 6, '5601', 1),
('Προηγμένες Αρχιτεκτονικές Υπολογιστών και Παράλληλα Συστήματα', 7, '5704', 1),
('Σημασιολογικός Ιστός', 7, '5710', 1),
('Συστήματα Διαχείρισης Βάσεων Δεδομένων', 3, '5305', 1),
('Τεχνητή Νοημοσύνη (Γλώσσες και Τεχνικές)', 4, '5401', 1),
('Τεχνολογία Βάσεων Δεδομένων', 4, '5404', 1),
('Τεχνολογία Πολυμέσων', 7, '5702', 1),
('Τηλεπικοινωνίες και Δίκτυα Υπολογιστών', 4, '5402', 1),
('Ψηφιακά Συστήματα', 1, '5103', 1);");
    }

    private static void insertProfessors(DbContext context)
    {
      context.Database.ExecuteSqlCommand(
        @"INSERT INTO professors (""isAdmin"", ""oAuthId"", ""firstName"", ""lastName"", ""office"", ""time"", ""phone"", ""fax"", ""email"") VALUES
(false,'','Παναγιώτης', 'Αδαμίδης', 'Γραφείο 106', 'Πέμπτη 11:00-12:00, Παρασκευή 13:00-14:00', '2310 013985', '2310 798256', 'adamidis@it.teithe.gr'),
(false,'','Ευστάθιος', 'Αντωνίου', 'Γραφείο καθηγητή', 'Δευτέρα 10:00-12:-00', '2310 013 429', '', 'test@test'),
(false,'','Αντώνιος', 'Σιδηρόπουλος','', '', '', '', 'asidirop@gmail.com'),
(false,'','Κωνσταντίνα', 'Χατζάρα', '', '', '', '', 'maragkosd12@gmail.com'),
(false,'','Δημήτρης', 'Δέρβος', 'Γραφείο 203', 'Δευτέρα 12:00-2:00 ', '2310.013999', '2310.798256', 'dad@it.teithe.gr'),
(false,'','Δημοσθένης', 'Σταμάτης', 'Τμήμα Πληροφορικής', 'Τρίτη 12:30 – 14:00 ', '2310 013025', '', 'demos@it.teithe.gr'),
(false,'4389','Δημήτριος', 'Αμανατιάδης','Γραφείο 207', 'Δευτέρα 14-16, Τρίτη 11-14', '2310013988', '2310798256', 'lalaladokimi'),
(false,'','Ευκλείδης', 'Κεραμόπουλος', 'Γραφείο 107', 'Δευτέρα 13:00, Παρασκευή 13:00', '2310013998', '', 'euclid@it.teithe.gr'),
(false,'','Κωνσταντίνος', 'Γουλιάνας', 'Γραφείο Επιβλέποντος', 'Δευτέρα 12:00-2:00 ', '2310.791287', '2310.791294', 'gouliana@it.teithe.gr'),
(false,'','Ιγνάτιος', 'Δεληγιάννης',  'ΑΤΕΙΘ', 'Δευτέρα 12:00-2:00 ', '2310013997', '', 'ignatios@it.teithe.gr'),
(false,'','Χρήστος', 'Ηλιούδης', 'Γραφείο', 'Δευτέρα 2-3 μμ', '231013022', '', 'iliou@it.teithe.gr'),
(false,'','Κωνσταντίνος', 'Διαμαντάρας', 'Αίθ. Τηλεκπαίδευσης', 'Τετάρτη 10-11', '2310 013592', '', 'kdiamant@it.teithe.gr'),
(false,'','Κωνσταντίνος', 'Γιακουστίδης', 'ΤΕΙΘ', '', '2310 791296 ', '', 'kgiak@it.teithe.gr'),
(false,'','Δημήτριος', 'Κλεφτούρης', 'Γραφείο Καθ.', 'Δευτέρα  13 -14,  Τετάρτη  10-12', '2310013299', '2310013290', 'klefturi@it.teithe.gr'),
(false,'','Εμμανουήλ', 'Βοζαλής',  '', '', '', '', ''),
(false,'','Μιχαήλ', 'Σαλαμπάσης',  'Θες/νίκη', '', '', '', 'msa@it.teithe.gr'),
(false,'','Περικλής', 'Χατζημίσιος', 'Τμήμα Πληροφορικής (ΑΤΕΙΘ), 1ος όροφος, γραφείο 206', 'Τετάρτη και ώρα 17:00-18:00 ', '2310 013024', '2310 798256 ', 'peris@it.teithe.gr'),
(false,'','Πασχάλης', 'Ράπτης', 'ΤΕΙΘ', '', '2310 791436', '', 'praptis@it.teithe.gr'),
(false,'','Νικόλαος', 'Ψαρράς', 'ΑΤΕΙΘ, Τμήμα Πληροφορικής', '', '2310 013013', '2310796256', 'psarnik@it.teithe.gr'),
(false,'','Παναγιώτης', 'Σαρηγιαννίδης',  '', '', '', '', ''),
(false,'','Παναγιώτης', 'Σφέτσος',  'Γραφείο', 'Τρίτη 10-11 π.μ', '2310-791436', '', 'sfetsos@it.teithe.gr'),
(false,'','Κέρστιν', 'Σιάκα',  'Θεσσαλονίκη', '', '2310 791296', '', 'siaka@it.teithe.gr'),
(false,'','Στέφανος', 'Ουγιάρογλου', 'Γραφείο 203', 'Δευτέρα 13:00-15:00, Πέμπτη 12:00-14:00', '2310 013926', '2310 798256', 'stoug@it.teithe.gr'),
(false,'','Βασίλειος', 'Βίτσας', 'Γραφείο 207', '', '2310-791299', '', 'vitsas@it.teithe.gr'),
(false,'','Βασίλειος', 'Κώστογλου', 'Γραφείο 212', 'Δευτέρα 12-14, Τρίτη 11-14, Τετάρτη 12-14. Κατόπιν προσυνεννόησης ', '2310.013294', '', 'vkostogl@it.teithe.gr');");
    }
  }
}
