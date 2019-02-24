namespace FToolsShared
{
    public class CustomPickupInfo
    {
        public int NetHandle { get; set; }
        public bool Dynamic { get; set; }
        public bool OnGround { get; set; }
        public bool DeleteOnAction { get; set; }
        public int EventActionType { get; set; }
        public int Control { get; set; }
        public string HelpText { get; set; }
        public dynamic CallBack { get; set; }
        public dynamic Parameters { get; set; }
        public bool Created { get; set; }
    }
}
