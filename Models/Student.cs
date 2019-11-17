using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace demoDotnet.Models {
  public class Student {
    [Key]
    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    [JsonProperty ("id")]
    public int Id { get; set; }

    [Required]
    [JsonProperty ("name")]
    public string Name { get; set; }

    [Required]
    [JsonProperty ("status")]
    public string Status { get; set; }

    [Required]
    [JsonProperty ("brithday")]
    public DateTime Brithday { get; set; }
  }
}