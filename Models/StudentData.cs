using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace demoDotnet.Models {
  public class StudentData {

    [JsonProperty ("student")]
    public Student Student;

    [JsonProperty ("classroomId")]
    public int ClassroomId;

    [JsonProperty ("classroom")]
    public List<Classroom> Classrooms;

    [JsonProperty ("genderId")]
    public int GenderId;

    [JsonProperty ("gender")]
    public List<Gender> Genders;
  }
}