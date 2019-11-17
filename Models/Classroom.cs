using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace demoDotnet.Models {
  public class Classroom {
    [Key]
    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    [JsonProperty ("id")]
    public int Id { get; set; }

    [Required]
    [JsonProperty ("name")]
    public string Name { get; set; }
  }
}