namespace AbsolCase.Configurations
{
    public static class Global_variable
    {
        public static double _listcart = -1;
        public static double listcart
        {
            set { _listcart = value; }
            get { return _listcart; }
        }
    }
}
