using System.Collections.Generic;
using System;

namespace PantryPlusRecipe.Models
{
  public class Step
  {
    public Step()
    {
      this.JoinEntities = new HashSet<StepRecipe>();
    }
    public int StepId { get; set; }
    public int StepNumber { get; set; }
    public string Details { get; set; }
    public int SectionNumber { get; set; }
    public string SectionName { get; set; }

    public virtual ICollection<StepRecipe> JoinEntities { get; }
    public virtual ApplicationUser User { get; set; }
  }
}
