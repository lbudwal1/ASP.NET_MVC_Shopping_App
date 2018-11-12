---
title: Customer Orders Design Plan
---
# Customer Orders - Design Plan

> **Notes:** Columns highlighted yellow on ERDs are for queries, while columns highlighted blue are for commands.

## Page Load

> **Customer Selection** (`<asp:DropDownList>`)

```csharp
// SalesController class
[DataObjectMethod(DataObjectMethodType.Select)]
public List<KeyValueOption> ListCustomerNames()
```

![](./images/Query-First-Visit.png)


## Selecting a Customer

> **Customer Summary Info** (disabled textboxes)

```csharp
// SalesController class
public CustomerSummary GetCustomerSummary(string customerId)
```

> **Order History Filters** (`<asp:RadioButtonList>`)

```csharp
// SalesController class
[DataObjectMethod(DataObjectMethodType.Select)]
public List<KeyValueOption> GetOrderHistoryFilters()
```

> **Order History** (`<asp:GridView>`)

```csharp
// SalesController class
[DataObjectMethod(DataObjectMethodType.Select)]
public List<CustomerOrder> GetOrdersByCustomer(string customerId, string filter)
```

![](./images/Query-Selected-Company.png)

----

## Starting New/Existing Order

Both the new and existing ("open") customer orders involve querying data from multiple tables.

![](./images/Query-Open-Order.png)

> **Shippers** (`<asp:DropDownList>`)

```csharp
[DataObjectMethod(DataObjectMethodType.Select)]
public List<KeyValueOption> GetShippers()
```

> **Add Products** (`<asp:DropDownList>`) - filtered to only show products not currently on the order

```csharp
// SalesController class
[DataObjectMethod(DataObjectMethodType.Select)]
public List<KeyValueOption> GetProducts()
```

### Existing Order

> **Select Existing Order** (GridView's `SelectedIndexChanged` event)

```csharp
// SalesController class
public CustomerOrderWithDetails GetExistingOrder(int orderId)
```

### New Order

> **New Order** button (`<asp:ListView>` is an empty list of `CustomerOrderWithDetails` POCO class)


----

## Saving Order

Saving the order means gathering all the order data from the form and calling a single BLL method to perform the transaction.

```csharp
// SalesController class
public void Save(EditCustomerOrder order)
```

![](./images/Command-Save-Order.png)

----

## Placing Customer Order

Placing the order also means gathering all the order data from the form and calling a single BLL method to perform a transaction.

```csharp
// SalesController class
public void PlaceOrder(EditCustomerOrder order)
```
