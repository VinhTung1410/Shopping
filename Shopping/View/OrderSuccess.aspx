<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderSuccess.aspx.cs" Inherits="Shopping.View.OrderSuccess" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card shadow-lg border-0 rounded-3 bg-white">
                    <div class="card-header bg-black text-white text-center py-4 border-0">
                        <h2 class="fw-bold mb-1">Order Success</h2>
                        <p class="mb-0 mt-2 text-light-emphasis">Thank you for your purchase</p>
                    </div>
                    <div class="card-body p-4 text-center">
                        <div class="success-icon mb-4">
                            <i class="fas fa-check-circle fa-4x text-success"></i>
                        </div>
                        <h3 class="fw-bold mb-3">Order Placed Successfully!</h3>
                        <p class="text-muted mb-4">
                            Your order has been received and is being processed.
                            You will receive an email confirmation shortly.
                        </p>
                        <div class="d-grid gap-2">
                            <asp:HyperLink ID="hlContinueShopping" runat="server" NavigateUrl="~/Default.aspx" 
                                CssClass="btn btn-dark btn-lg fw-semibold">
                                <i class="fas fa-shopping-cart me-2"></i>Continue Shopping
                            </asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        body {
            background-color: #f8f9fa !important;
        }
        .card {
            border-radius: 1rem;
            transition: all 0.3s ease-in-out;
        }
        .card:hover {
            transform: translateY(-5px);
            box-shadow: 0 1rem 2rem rgba(0, 0, 0, 0.1) !important;
        }
        .card-header {
            border-radius: 1rem 1rem 0 0 !important;
        }
        .btn-dark {
            transition: all 0.3s ease;
            padding: 0.8rem;
            letter-spacing: 0.5px;
        }
        .btn-dark:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }
        .text-light-emphasis {
            color: rgba(255, 255, 255, 0.8) !important;
        }
        .success-icon {
            color: #28a745;
        }
    </style>
</asp:Content>
