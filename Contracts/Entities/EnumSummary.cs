namespace Contracts.Entities
{
    public class EnumSummary<T> where T : struct //, IConvertible
    {
        public string ID
        {
            get { return this.Status.ToString(); }
        }

        public int Value
        {
            get
            {
                //if (!typeof(T).IsEnum) return 0;
                foreach (var item in System.Enum.GetValues(typeof(T)))
                {
                    if (item.ToString() == this.Status.ToString()) return (int)item;
                }
                return 0;
            }
        }

        public string Name { get; set; }

        public T Status;

        public EnumSummary(T status, string name)
        {
            this.Status = status;
            this.Name = name;
        }
    }
}