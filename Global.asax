<%@ Application Language="C#" %>

<script runat="server">
    void Application_BeginRequest(object sender, EventArgs e)
    {
        HttpContext context = HttpContext.Current;
        if (context == null)
        {
            return;
        }

        HttpResponse response = context.Response;
        response.ContentEncoding = System.Text.Encoding.UTF8;
        response.Charset = "utf-8";

        if (response.ContentType == "text/html" || string.IsNullOrEmpty(response.ContentType))
        {
            response.ContentType = "text/html";
        }
    }
</script>
