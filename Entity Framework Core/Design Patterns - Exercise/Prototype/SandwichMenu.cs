namespace Prototype
{
    using System.Collections.Generic;

    public class SandwichMenu
    {
        private Dictionary<string, SandwichPrototype> sandwiches = new Dictionary<string, SandwichPrototype>();

        public SandwichPrototype this[string name]
        {
            get { return this.sandwiches[name]; }
            set { this.sandwiches.Add(name, value); }
        }

        public Dictionary<string, SandwichPrototype> GetAllSandwiches()
        {
            return this.sandwiches;
        } 
    }
}