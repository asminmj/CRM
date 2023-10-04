using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B.Models.B1
{
  [Table("Citizenship")]
  public partial class Citizenship
  {
    [Key]
    public int ID
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string Name
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string Birthplace
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string District
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int Ward
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string Birthdate
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string FatherName
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string Address
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int CitizenshipNumber
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string PermanentAddrress
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string Gender
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string CitizenshipRegisterDate
    {
      get;
      set;
    }
  }
}
