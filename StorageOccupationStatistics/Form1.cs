namespace StorageOccupationStatistics {
    public partial class Form1 : Form {
        // it is accurate but not when on the C
        // We need to find the folder that makes it calculate wrong by trying each forder alone in the C

        // 1- implement save and load
        // 2- implement the choose heirarchy collapsable table
        // 3- add the pie and bar chart GUI
        // 4- make that GUI clickable


        public Form1() {
            InitializeComponent();
            filePanel.AutoScroll = true;
            //FileInfoTree fit = new FileInfoTree("C:\\Program Files\\Adobe", filePanel);
            FileInfoTree fit = new FileInfoTree("C:\\Program Files\\", filePanel);
            //fit.GetRoot().Expand();
            //fit.GetRoot().ExpandChildren();
            fit.DrawUI();
            
        }
        // what about C# table with the first column having button and label?

        private void button1_Click(object sender, EventArgs e) {

        }
    }
}