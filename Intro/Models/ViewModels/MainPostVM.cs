namespace Intro.Models.ViewModels
{
    public class MainPostVM
    {
        public MainPostVM()
        {
            Blog = new Blog();
        }
        public Blog Blog { get; set; }
    }
}
