using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace demoDotnet.Models {
  public class StudentList {

    [JsonProperty ("id")]
    public int Id;

    [JsonProperty ("name")]
    public string Name;

    [JsonProperty ("status")]
    public string Status;

    [JsonProperty ("brithday")]
    public DateTime Brithday;

    [JsonProperty ("classroomName")]
    public string ClassroomName;

    [JsonProperty ("genderName")]
    public string GenderName;
  }
}