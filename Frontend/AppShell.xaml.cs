namespace Frontend {
    public partial class AppShell : Shell {
        public AppShell() {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddDamagePage), typeof(AddDamagePage));
        }
    }
}
