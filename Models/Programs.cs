// using System;
// using System.ComponentModel.DataAnnotations;

// namespace ClassScheduling_WebApp.Models
// {
//   public class Programs
//   {
//     [Key]
//     public int id { get; set; }

//     [Required]
//     [MaxLength(100, ErrorMessage = "Please enter a shorter program name")]
//     public string programName { get; set; }
//   }

//   public class ProgramData
//   {
//     // array of program names
//     public static string[] programs = new string[]
//     {
//             "IT Web Programming Year 1",
//             "IT Web Programming Year 2",
//             "IT Systems Management Year 1",
//             "IT Systems Management Year 2"
//     };
//   }
// }