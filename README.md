# AzSalesOrderWebHook

This example shows a simple SMS Integration by using Azure Functions and Business Central WebHooks.

SCENARIO

1. On Business Central, when a sales order is being updated the subscribed WebHook is invoked;
2. The Webhook gets order details and send a SMS to the sale agent (eg: "The XYZ order has been released, do you want to send a confirmation email to your customer"?) and wait his/her reply.
3. The sale agent reply with a YES/NO and in case the Azure Function sends an SMS with order detail to the final customer; 

The description of the project:
https://www.business-central.blog/2020/04/12/send-an-event-trigger-sms-by-using-azure-functions-and-the-powerful-bc-webhooks/# AzBcSalesOrderPayPal
