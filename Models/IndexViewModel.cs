using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassScheduling_WebApp.Models
{
  public class IndexViewModel
  {
    //the purpose of this class is to provide a model for the Index view that can contain lists of all the programs courses and technologies for crud methods to be used in the view.
    public List<ProgramModel> Programs { get; set; }
    public List<TechnologyModel> Technologies { get; set; }
    public List<UserModel> Users { get; set; }
  }
}