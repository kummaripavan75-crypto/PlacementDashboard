
<%@ Page Language="C#" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    DataTable dt;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dt = new DataTable();

            dt.Columns.Add("RollNo");
            dt.Columns.Add("Name");
            dt.Columns.Add("Company");
            dt.Columns.Add("Status");

            dt.Rows.Add("101", "Pavan", "TCS", "Placed");
            dt.Rows.Add("102", "Rahul", "Infosys", "Placed");
            dt.Rows.Add("103", "Arun", "-", "Not Placed");

            ViewState["Students"] = dt;

            BindData();
        }
    }

    void BindData()
    {
        dt = (DataTable)ViewState["Students"];

        GridView1.DataSource = dt;
        GridView1.DataBind();

        int total = dt.Rows.Count;
        int placed = 0;

        foreach (DataRow row in dt.Rows)
        {
            if (row["Status"].ToString() == "Placed")
                placed++;
        }

        lblTotal.Text = total.ToString();
        lblPlaced.Text = placed.ToString();
        lblNotPlaced.Text = (total - placed).ToString();

        double percentage = total == 0 ? 0 :
            (placed * 100.0) / total;

        lblPercentage.Text = percentage.ToString("0.00") + "%";
    }

    protected void AddStudent(object sender, EventArgs e)
    {
        dt = (DataTable)ViewState["Students"];

        if (txtName.Text == "" || txtRoll.Text == "")
        {
            lblMessage.Text = "Enter student details!";
            return;
        }

        dt.Rows.Add(
            txtRoll.Text,
            txtName.Text,
            ddlStatus.SelectedValue == "Placed" ? txtCompany.Text : "-",
            ddlStatus.SelectedValue
        );

        ViewState["Students"] = dt;

        BindData();

        txtName.Text = "";
        txtRoll.Text = "";
        txtCompany.Text = "";

        lblMessage.Text = "Student Added Successfully!";
    }

    protected void DeleteStudent(object sender, GridViewDeleteEventArgs e)
    {
        dt = (DataTable)ViewState["Students"];

        dt.Rows.RemoveAt(e.RowIndex);

        ViewState["Students"] = dt;

        BindData();
    }

    protected void GenerateReport(object sender, EventArgs e)
    {
        dt = (DataTable)ViewState["Students"];

        DataTable report = new DataTable();

        report.Columns.Add("Report");
        report.Columns.Add("Count");

        int placed = 0;

        foreach (DataRow row in dt.Rows)
        {
            if (row["Status"].ToString() == "Placed")
                placed++;
        }

        report.Rows.Add("Total Students", dt.Rows.Count);
        report.Rows.Add("Placed Students", placed);
        report.Rows.Add("Not Placed Students", dt.Rows.Count - placed);

        GridViewReport.DataSource = report;
        GridViewReport.DataBind();
    }

</script>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Placement Dashboard</title>

    <style>
        body {
            font-family: Arial;
            background: #f1f4f9;
            margin: 0;
        }

        .header {
            background: #243b55;
            color: white;
            text-align: center;
            padding: 20px;
        }

        .container {
            padding: 25px;
        }

        .card {
            display: inline-block;
            background: white;
            padding: 20px;
            width: 25%;
            margin: 10px;
            text-align: center;
            border-radius: 8px;
        }

        .card h2 {
            color: #243b55;
        }

        .section {
            background: white;
            padding: 20px;
            margin-top: 20px;
            border-radius: 8px;
        }

        input, select {
            padding: 10px;
            margin: 8px;
        }

        .btn {
            background: #243b55;
            color: white;
            padding: 10px 18px;
            border: none;
            cursor: pointer;
        }

        table {
            margin-top: 15px;
        }

        .message {
            color: green;
            font-weight: bold;
        }
    </style>

</head>

<body>

<form id="form1" runat="server">

    <div class="header">
        <h1>College Placement Dashboard</h1>
        <p>Student Placement Management System</p>
    </div>

    <div class="container">

        <div class="card">
            <h3>Total Students</h3>
            <h2><asp:Label ID="lblTotal" runat="server" /></h2>
        </div>

        <div class="card">
            <h3>Placed Students</h3>
            <h2><asp:Label ID="lblPlaced" runat="server" /></h2>
        </div>

        <div class="card">
            <h3>Not Placed</h3>
            <h2><asp:Label ID="lblNotPlaced" runat="server" /></h2>
        </div>

        <div class="section">

            <h2>Add Student Details</h2>

            <asp:TextBox ID="txtName" runat="server"
                placeholder="Student Name" />

            <asp:TextBox ID="txtRoll" runat="server"
                placeholder="Roll Number" />

            <asp:TextBox ID="txtCompany" runat="server"
                placeholder="Company Name" />

            <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem>Placed</asp:ListItem>
                <asp:ListItem>Not Placed</asp:ListItem>
            </asp:DropDownList>

            <asp:Button ID="btnAdd" runat="server"
                Text="Add Student"
                CssClass="btn"
                OnClick="AddStudent" />

            <br /><br />

            <asp:Label ID="lblMessage" runat="server"
                CssClass="message" />

        </div>

        <div class="section">

            <h2>Student Placement Details</h2>

            <asp:GridView ID="GridView1" runat="server"
                AutoGenerateColumns="False"
                CellPadding="10"
                BackColor="White"
                GridLines="Both"
                OnRowDeleting="DeleteStudent">

                <Columns>

                    <asp:BoundField DataField="RollNo"
                        HeaderText="Roll Number" />

                    <asp:BoundField DataField="Name"
                        HeaderText="Student Name" />

                    <asp:BoundField DataField="Company"
                        HeaderText="Company" />

                    <asp:BoundField DataField="Status"
                        HeaderText="Status" />

                    <asp:CommandField ShowDeleteButton="True"
                        ButtonType="Button" />

                </Columns>

            </asp:GridView>

        </div>

        <div class="section">

            <h2>Placement Reports</h2>

            <h3>
                Placement Percentage:
                <asp:Label ID="lblPercentage" runat="server" />
            </h3>

            <asp:Button ID="btnReport" runat="server"
                Text="Generate Report"
                CssClass="btn"
                OnClick="GenerateReport" />

            <asp:GridView ID="GridViewReport" runat="server"
                AutoGenerateColumns="True"
                CellPadding="10"
                BackColor="White" />

        </div>

    </div>

</form>

</body>
</html>
