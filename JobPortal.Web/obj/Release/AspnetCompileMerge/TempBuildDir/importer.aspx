<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="importer.aspx.cs" Inherits="JobPortal.Web.importer" %>
<!DOCTYPE html>
<html lang="en">
    <head runat="server">
    <title>SocialInviter - Contact Importer</title>
    <link rel="stylesheet" type="text/css" href="contactimporter.css">
	<script src="//socialinviter.com/assets/js/libraries/jquery.js"></script>
	<script type="text/javascript" id="apiscript" src="contactimporter.js?key=lic_ea2b7167-f480-41d0-b579-02af3"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="addBook" value="" runat="server"/>
    <input type="hidden" id="selectedcontacts" value="" runat="server"/>
    <input type="hidden" id="subject" value="" runat="server"/>
    <input type="hidden" id="message" value="" runat="server"/>
    <div>
    
    <!-- Makes Ajax call to codebehind file -->
    <asp:scriptmanager runat="server"></asp:scriptmanager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:imagebutton runat="server" id="btnimportcontacts" style="width:0px;height:0px;position:absolute;left:-1000px" onclick="btnimportcontacts_Click"></asp:imagebutton>
            <asp:imagebutton runat="server" id="btnsend" style="width:0px;height:0px;position:absolute;left:-1000px" onclick="btnsend_Click"></asp:imagebutton>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- ends -->
    </div>
    </form>
    
     <div id="socialinviter-CI"></div><br />
     <!-- The below div must be outside the form tag -->
     <div id="socialinviter-CI-template"></div>

     <!-- Place the below script at the end of the file -->
     <script type="text/javascript">
         var storeImportedContacts = function () {
             document.getElementById("<%= addBook.ClientID %>").value = JSON.stringify(contactimporter.getAllContacts().addressbook);
             document.getElementById("<%= btnimportcontacts.ClientID %>").click();
         }
         var sendSelectedContacts = function () {
             document.getElementById("<%= selectedcontacts.ClientID %>").value = JSON.stringify(contactimporter.getRecipients());
             document.getElementById("<%= subject.ClientID %>").value = $(".mailing-subject").val();
             document.getElementById("<%= message.ClientID %>").value = $(".mailing-message").val();
             document.getElementById("<%= btnsend.ClientID %>").click();
         }
         var sendConfirmation = function () {
             modalSI.showSuccessMessage("Success: Email sent.");
         }
     </script>
    
</body>
</html>