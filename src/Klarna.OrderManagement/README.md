EPiServer Klarna order management
=============

## What is Klarna.OrderManagement?

Klarna.OrderManagement is a library for processing a Klarna payment in EPiServer Commerce. This package supports both payments of Klarna.Payments and Klarna.Checkout. 
More about Klarna ordermanagement: https://developers.klarna.com/en/gb/kco-v3/order-management

### Steps integrated with EPiServer Commerce
- **Capture** - either partially (multi-shipment) or full capture the payment amount
- **Release remaining authorization** - release remaining authorization when the payment amount has not been fully captured
- **Refund** - either partially or full refund an amount
- **Cancel** - cancel payment

### Other steps only available in the (*)code
- **Get Klarna order**
- **Update merchant reference** - update merchant reference 1 and 2
- **Trigger send out**
- **Extend authorization time**
- **Update customer information**

(*) not integrated in EPiServer Commerce

### Capture

### Release remaining authorization
In a multi-shipment scenario, each individual shipment can be completed or cancelled. Say for instance we've an order with two shipments, one shipments was fullfilled and the other one was cancelled (partially completed). This means the remaining authorized amount at Klarna needs to be released. 

![Order multi shipment](/docs/screenshots/order-multi-shipment.PNG?raw=true "Order multi shipment")

When the last shipment is handled the payment gateway is called to release the remaining authorized amount at Klarna.

![Order release remaining authorization](/docs/screenshots/order-payment-releaseremainingauthorization.PNG?raw=true "Order release remaining authorization")

### Refund
To create a return in Commerce Manager the order must have the completed status. Follow these steps to create a return:
- Open the order in Commerce Manager
- Go to the Details tab
- Press the 'Create return' button
- New popup window is opened, add order lines, some comments and finally press 'Save'
![Order create return](/docs/screenshots/order-create-return.PNG?raw=true "Order create return")
- Got ot the Returns tab
- Press the 'Acknowledge Receipt Items' button
- To complete the return press the 'Complete button'

When the return is completed the payment gateway is called to create a refund at Klarna. In the Payments tab, an extra row for the payment refund (called Credit in Commerce Manager) has been added. Also, a note is added at the order.

![Order payments refund](/docs/screenshots/order-payments-refund.PNG?raw=true "Order payments refund")

### Cancel
Whenever an order is cancelled in Commerce Manager the payment gateway is called to alos cancel the payment at Klarna.
An order in Commerce Manager can only be can cancelled when the items haven't been shipped yet. 
![Cancel order](/docs/screenshots/order-cancel.PNG?raw=true "Cancel order")

After the cancel button is pressed the payment gateway is called. The passed payment object contains the transaction type 'Void' which means the payment should be cancelled. This is also what happens at Klarna.
![Order payments void](/docs/screenshots/order-payments-void.PNG?raw=true "Order payments void")

### Order notes
Order notes are used for payment gateway updates.


### Klarna order information
Order notes and the payment overview can be used to gather information about the Klarna payment process. The Payments tab contains more information about the order (payment) at Klarna. By clicking on the 'Show all order information' link a complete JSON of the order object from Klarna is displayed. 

Note: this information is only displayed  when a Klarna payment is added to the order in Commerce Manager.

![Klarna order information](/docs/screenshots/order-klarna-information.PNG?raw=true "Klarna order information")