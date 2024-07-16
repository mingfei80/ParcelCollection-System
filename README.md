## Programming Exercise - Parcel Collection Centre (Platform Team)

I am creating a service that calculates the estimated collection dates of customers' orders. 

An order consists of an order date and the products a customer has added to their shopping basket. 

These products are supplied to the PayPoint Collection Centre on demand through several 3rd party suppliers.

When a supplier receives an order, it starts processing it. The supplier has an agreed-upon lead time for processing the order before delivering it to the PayPoint Collection Centre.

Once the PayPoint Collection Centre has received all products in an order, it is ready for the customer to collect.

**Assumptions**:

1. Suppliers start processing an order on the same day that the order is received. For example, a supplier with a lead time of one day receiving an order today will send it to the PayPoint Collection Centre tomorrow.

2. For this exercise, we are ignoring time. If the supplier has a lead time of 1 day, then an order received any time on Tuesday would arrive at the PayPoint Collection Centre on Wednesday.

3. Once all the products for an order have arrived at the PayPoint Collection Centre from the suppliers, the customer will collect them on the same day.
	
4. Any orders received from a supplier on the weekend are they will start the process on the following Monday.	

5. Any orders that would have been processed during the weekend resume processing on Monday.

## Request and Response Examples

Please see examples of how to make requests and the expected response below.


### Request

The service is setup as a Web API and takes a request in the following format

~~~~ 
GET /api/CollectionDate?ProductIds={product_id}&orderDate={order_date} 
~~~~

e.g.

~~~~ 
GET /api/CollectionDate?ProductIds=1&orderDate=2018-01-29T00:00:00
GET /api/CollectionDate?ProductIds=2&ProductIds=3&orderDate=2018-01-29T00:00:00 
~~~~

### Response

The response will be a JSON object with a date property set to the resulting 
Collection Date

~~~~ 
{
    "date" : "2018-01-30T00:00:00"
}
~~~~ 

## Acceptance Criteria

### Lead time added to collection date  

**Given** An order includes a product from a supplier with a guaranteed lead time of just 1 day  
**And** the order is place on a Monday - 01/01/2018  
**When** the collection date is calculated  
**Then** the collection date is Tuesday - 02/01/2018  

**Given** An order includes a product from a supplier with a guaranteed lead time of just 2 days  
**And** the order is place on a Monday - 01/01/2018  
**When** the collection date is calculated  
**Then** the collection date is Wednesday - 03/01/2018  

### Supplier with longest lead time is used for calculation

**Given** An order includes a product from a supplier with a guaranteed lead time of just 1 day  
**And** the order also contains a product from a different supplier with a lead time of 2 days  
**And** the order is place on a Monday - 01/01/2018  
**When** the collection date is calculated  
**Then** the collection date is Wednesday - 03/01/2018  

### Lead time is not counted over a weekend

**Given** An order includes a product from a supplier with a guaranteed lead time of just 1 day  
**And** the order is place on a Friday - 05/01/2018  
**When** the collection date is calculated  
**Then** the collection date is Monday - 08/01/2018  

**Given** An order includes a product from a supplier with a guaranteed lead time of just 1 day  
**And** the order is place on a Saturday - 06/01/18  
**When** the collection date is calculated  
**Then** the collection date is Tuesday - 09/01/2018  

**Given** An order includes a product from a supplier with a guaranteed lead time of just 1 days  
**And** the order is place on a Sunday - 07/01/2018  
**When** the collection date is calculated  
**Then** the collection date is Tuesday - 09/01/2018  

### Lead time over multiple weeks

**Given** An order includes a product from a supplier with a guaranteed lead time of just 6 days  
**And** the order is place on a Friday - 05/01/2018  
**When** the collection date is calculated  
**Then** the collection date is Monday - 15/01/2018  

**Given** An order includes a product from a supplier with a guaranteed lead time of just 11 days  
**And** the order is place on a Friday - 05/01/2018  
**When** the collection date is calculated  
**Then** the collection date is Monday - 22/01/2018
