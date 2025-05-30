<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Adminpage.aspx.cs" Inherits="Shopping.Admin.Adminpage" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderStyles" runat="server">
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <!-- DataTables -->
        <link href="https://cdn.jsdelivr.net/npm/simple-datatables@7.1.2/dist/style.min.css" rel="stylesheet" />
    <!-- Custom styles -->
    <link href='<%=ResolveUrl("~/Admin/css/styles.css")%>' rel="stylesheet" type="text/css" />
    
    <style>
        #layoutSidenav {
            display: flex;
        }
        
        #layoutSidenav_nav {
            flex-basis: 225px;
            flex-shrink: 0;
            transition: transform .15s ease-in-out;
            z-index: 1038;
            transform: translateX(0);
        }

        .sb-sidenav-dark {
            background-color: #212529;
            color: rgba(255, 255, 255, 0.5);
        }

        .sb-sidenav {
            display: flex;
            flex-direction: column;
            height: 100%;
            flex-wrap: nowrap;
        }

        .sb-sidenav .sb-sidenav-menu {
            flex-grow: 1;
        }

        .sb-sidenav .sb-sidenav-menu .nav {
            flex-direction: column;
            flex-wrap: nowrap;
        }

        .sb-sidenav-dark .sb-sidenav-menu .nav-link {
            color: rgba(255, 255, 255, 0.5);
            padding: 0.75rem 1rem;
            display: flex;
            align-items: center;
        }

        .sb-sidenav-dark .sb-sidenav-menu .nav-link:hover {
            color: #fff;
            background-color: rgba(255, 255, 255, 0.1);
        }

        .sb-sidenav-dark .sb-sidenav-menu .nav-link.active {
            color: #fff;
            background-color: rgba(255, 255, 255, 0.15);
        }

        .sb-sidenav-menu-heading {
            padding: 1.75rem 1rem 0.75rem;
            font-size: 0.75rem;
            font-weight: bold;
            text-transform: uppercase;
            color: rgba(255, 255, 255, 0.4);
        }

        .sb-nav-link-icon {
            margin-right: 0.5rem;
            width: 1rem;
            text-align: center;
        }

        .sb-sidenav-footer {
            padding: 0.75rem;
            background-color: #343a40;
        }

        /* Navbar styles */
        .sb-topnav {
            padding-left: 0;
            height: 56px;
            z-index: 1039;
        }

        .sb-topnav.navbar-dark #sidebarToggle {
            color: rgba(255, 255, 255, 0.5);
        }

        .sb-topnav.navbar-dark #sidebarToggle:hover {
            color: #fff;
        }

        /* Profile Dropdown */
        .dropdown-profile .dropdown-toggle::after {
            display: none;
        }

        .dropdown-profile .dropdown-menu {
            min-width: 15rem;
            padding: 0.5rem 0;
            margin: 0;
            right: 0;
            left: auto;
        }

        .dropdown-profile .dropdown-item {
            padding: 0.5rem 1rem;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }

        .dropdown-profile .dropdown-item:hover {
            background-color: #f8f9fa;
        }

        .dropdown-profile .dropdown-divider {
            margin: 0.5rem 0;
        }

        #layoutSidenav_content {
            padding-left: 225px;
            flex: 1 0 auto;
            display: flex;
            flex-direction: column;
        }

        @media (max-width: 768px) {
            #layoutSidenav_nav {
                transform: translateX(-225px);
            }
            
            .sb-sidenav-toggled #layoutSidenav_nav {
                transform: translateX(0);
            }
            
            #layoutSidenav_content {
                padding-left: 0;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="NavbarContent" ContentPlaceHolderID="NavContent" runat="server">
    <nav class="sb-topnav navbar navbar-expand navbar-dark bg-dark">
        <!-- Navbar Brand-->
        <a class="navbar-brand ps-3" href="Adminpage.aspx">Shop Admin</a>
        <!-- Sidebar Toggle-->
        <button class="btn btn-link btn-sm order-1 order-lg-0 me-4 me-lg-0" id="sidebarToggle">
            <i class="fas fa-bars"></i>
        </button>
        <!-- Navbar Search-->
        <div class="d-none d-md-inline-block form-inline ms-auto me-0 me-md-3 my-2 my-md-0">
            <div class="input-group">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search for..." />
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
            </div>
        </div>
        <!-- Navbar Profile-->
        <ul class="navbar-nav ms-auto ms-md-0 me-3 me-lg-4">
            <li class="nav-item dropdown dropdown-profile">
                <a class="nav-link dropdown-toggle" id="navbarDropdown" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="fas fa-user fa-fw"></i>
                </a>
                <ul class="dropdown-menu dropdown-menu-end shadow" aria-labelledby="navbarDropdown">
                    <li>
                        <a class="dropdown-item" href="Profile.aspx">
                            <i class="fas fa-user-circle fa-fw"></i>
                            Profile
                        </a>
                    </li>
                    <li><hr class="dropdown-divider" /></li>
                    <li>
                        <asp:LinkButton ID="btnLogout" runat="server" CssClass="dropdown-item text-danger" OnClick="btnLogout_Click">
                            <i class="fas fa-sign-out-alt fa-fw"></i>
                            Logout
                        </asp:LinkButton>
                    </li>
                </ul>
            </li>
        </ul>
    </nav>

    <div id="layoutSidenav">
        <div id="layoutSidenav_nav">
            <nav class="sb-sidenav accordion sb-sidenav-dark" id="sidenavAccordion">
                <div class="sb-sidenav-menu">
                    <div class="nav">
                        <div class="sb-sidenav-menu-heading">Core</div>
                        <a class="nav-link" href="Adminpage.aspx">
                            <div class="sb-nav-link-icon"><i class="fas fa-tachometer-alt"></i></div>
                            Dashboard
                        </a>
                        
                        <div class="sb-sidenav-menu-heading">Interface</div>
                        <a class="nav-link collapsed" href="#" data-bs-toggle="collapse" data-bs-target="#collapseProducts">
                            <div class="sb-nav-link-icon"><i class="fas fa-columns"></i></div>
                            Products
                            <div class="ms-auto">
                                <i class="fas fa-angle-down"></i>
                            </div>
                        </a>
                        <div class="collapse" id="collapseProducts" data-bs-parent="#sidenavAccordion">
                            <nav class="sb-sidenav-menu-nested nav">
                                <a class="nav-link" href="ProductList.aspx">Product List</a>
                                <a class="nav-link" href="AddProduct.aspx">Add Product</a>
                                <a class="nav-link" href="Categories.aspx">Categories</a>
                            </nav>
                        </div>

                        <a class="nav-link collapsed" href="#" data-bs-toggle="collapse" data-bs-target="#collapseOrders">
                            <div class="sb-nav-link-icon"><i class="fas fa-book-open"></i></div>
                            Orders
                            <div class="ms-auto">
                                <i class="fas fa-angle-down"></i>
                            </div>
                        </a>
                        <div class="collapse" id="collapseOrders" data-bs-parent="#sidenavAccordion">
                            <nav class="sb-sidenav-menu-nested nav">
                                <a class="nav-link" href="OrderList.aspx">Order List</a>
                                <a class="nav-link" href="PendingOrders.aspx">Pending Orders</a>
                                <a class="nav-link" href="CompletedOrders.aspx">Completed Orders</a>
                            </nav>
                        </div>

                        <div class="sb-sidenav-menu-heading">Addons</div>
                        <a class="nav-link" href="Users.aspx">
                            <div class="sb-nav-link-icon"><i class="fas fa-users"></i></div>
                            Users
                        </a>
                        <a class="nav-link" href="Reports.aspx">
                            <div class="sb-nav-link-icon"><i class="fas fa-chart-area"></i></div>
                            Reports
                        </a>
                        <a class="nav-link" href="Settings.aspx">
                            <div class="sb-nav-link-icon"><i class="fas fa-cog"></i></div>
                            Settings
                        </a>
                    </div>
                </div>
                <div class="sb-sidenav-footer">
                    <div class="small">Logged in as:</div>
                    <asp:Label ID="lblLoggedInUser" runat="server" CssClass="text-white" />
                </div>
            </nav>
        </div>

        <div id="layoutSidenav_content">
            <main>
                <div class="container-fluid px-4">
                    <h1 class="mt-4">Dashboard</h1>
                    <ol class="breadcrumb mb-4">
                        <li class="breadcrumb-item active">Dashboard</li>
                    </ol>
                
                <!-- Dashboard Cards -->
                    <div class="row">
                        <div class="col-xl-3 col-md-6">
                            <div class="card bg-primary text-white mb-4">
                            <div class="card-body">
                                <asp:Label ID="lblTotalOrders" runat="server" Text="Total Orders" />
                                <h2><asp:Label ID="lblOrderCount" runat="server" Text="0" /></h2>
                            </div>
                                <div class="card-footer d-flex align-items-center justify-content-between">
                                <a class="small text-white stretched-link" href="OrderList.aspx">View Details</a>
                                    <div class="small text-white"><i class="fas fa-angle-right"></i></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-md-6">
                            <div class="card bg-warning text-white mb-4">
                            <div class="card-body">
                                <asp:Label ID="lblTotalProducts" runat="server" Text="Total Products" />
                                <h2><asp:Label ID="lblProductCount" runat="server" Text="0" /></h2>
                            </div>
                                <div class="card-footer d-flex align-items-center justify-content-between">
                                <a class="small text-white stretched-link" href="ProductList.aspx">View Details</a>
                                    <div class="small text-white"><i class="fas fa-angle-right"></i></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-md-6">
                            <div class="card bg-success text-white mb-4">
                            <div class="card-body">
                                <asp:Label ID="lblTotalRevenue" runat="server" Text="Total Revenue" />
                                <h2><asp:Label ID="lblRevenueAmount" runat="server" Text="$0" /></h2>
                            </div>
                                <div class="card-footer d-flex align-items-center justify-content-between">
                                <a class="small text-white stretched-link" href="Reports.aspx">View Details</a>
                                    <div class="small text-white"><i class="fas fa-angle-right"></i></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-md-6">
                            <div class="card bg-danger text-white mb-4">
                            <div class="card-body">
                                <asp:Label ID="lblTotalUsers" runat="server" Text="Total Users" />
                                <h2><asp:Label ID="lblUserCount" runat="server" Text="0" /></h2>
                            </div>
                                <div class="card-footer d-flex align-items-center justify-content-between">
                                <a class="small text-white stretched-link" href="Users.aspx">View Details</a>
                                    <div class="small text-white"><i class="fas fa-angle-right"></i></div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Charts -->
                    <div class="row">
                        <div class="col-xl-6">
                            <div class="card mb-4">
                                <div class="card-header">
                                    <i class="fas fa-chart-area me-1"></i>
                                Monthly Revenue
                                </div>
                            <div class="card-body">
                                <canvas id="myAreaChart" width="100%" height="40"></canvas>
                            </div>
                            </div>
                        </div>
                        <div class="col-xl-6">
                            <div class="card mb-4">
                                <div class="card-header">
                                    <i class="fas fa-chart-bar me-1"></i>
                                Monthly Orders
                                </div>
                            <div class="card-body">
                                <canvas id="myBarChart" width="100%" height="40"></canvas>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- DataTable -->
                    <div class="card mb-4">
                        <div class="card-header">
                            <i class="fas fa-table me-1"></i>
                        Recent Orders
                        </div>
                        <div class="card-body">
                        <asp:GridView ID="gvOrders" runat="server" CssClass="table table-bordered table-striped" 
                            AutoGenerateColumns="False" OnRowCommand="gvOrders_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Amount" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnView" runat="server" CssClass="btn btn-primary btn-sm" CommandName="ViewOrder" 
                                            CommandArgument='<%# Eval("OrderID") %>'>
                                            <i class="fas fa-eye"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-sm" CommandName="EditOrder" 
                                            CommandArgument='<%# Eval("OrderID") %>'>
                                            <i class="fas fa-edit"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        </div>
                    </div>
                </div>
            </main>

        <!-- Footer -->
            <footer class="py-4 bg-light mt-auto">
                <div class="container-fluid px-4">
                    <div class="d-flex align-items-center justify-content-between small">
                    <div class="text-muted">Copyright &copy; Your Website 2024</div>
                        <div>
                            <a href="#">Privacy Policy</a>
                            &middot;
                            <a href="#">Terms &amp; Conditions</a>
                        </div>
                    </div>
                </div>
            </footer>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptsContent" ContentPlaceHolderID="Scripts" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.8.0/Chart.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/simple-datatables@7.1.2/dist/umd/simple-datatables.min.js"></script>
    <script src='<%=ResolveUrl("~/Admin/js/scripts.js")%>'></script>
    <script src='<%=ResolveUrl("~/Admin/js/chart-area-demo.js")%>'></script>
    <script src='<%=ResolveUrl("~/Admin/js/chart-bar-demo.js")%>'></script>
    <script src='<%=ResolveUrl("~/Admin/js/datatables-simple-demo.js")%>'></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        document.addEventListener('DOMContentLoaded', function() {
            // Sidebar Toggle
            var sidebarToggle = document.getElementById('sidebarToggle');
            if (sidebarToggle) {
                sidebarToggle.addEventListener('click', function(e) {
                    e.preventDefault();
                    document.body.classList.toggle('sb-sidenav-toggled');
                });
            }

            // Initialize dropdowns
            var dropdownElementList = [].slice.call(document.querySelectorAll('[data-bs-toggle="dropdown"]'))
            dropdownElementList.map(function (dropdownToggleEl) {
                return new bootstrap.Dropdown(dropdownToggleEl)
            });

            // Initialize collapses
            var collapseElementList = [].slice.call(document.querySelectorAll('[data-bs-toggle="collapse"]'))
            collapseElementList.map(function (collapseEl) {
                return new bootstrap.Collapse(collapseEl, {
                    toggle: false
                })
            });

            // Add active class to current page
            var currentPage = window.location.pathname.split('/').pop();
            var navLinks = document.querySelectorAll('.nav-link');
            navLinks.forEach(function(link) {
                if (link.getAttribute('href') === currentPage) {
                    link.classList.add('active');
                    var parentCollapse = link.closest('.collapse');
                    if (parentCollapse) {
                        parentCollapse.classList.add('show');
                        var trigger = document.querySelector('[data-bs-target="#' + parentCollapse.id + '"]');
                        if (trigger) {
                            trigger.classList.remove('collapsed');
                        }
                    }
                }
            });
        });
    </script>
</asp:Content>
