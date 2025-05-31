<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Admin/AdminLayout.master" AutoEventWireup="true" CodeBehind="Adminpage.aspx.cs" Inherits="Shopping.Admin.Adminpage" %>

<asp:Content ID="DashboardContent" ContentPlaceHolderID="MainContent" runat="server">
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
</asp:Content>
