# Entity-Relationship Diagram (ERD)

```mermaid
graph TD
    subgraph Entities
        Users -->|1| Wallet
        Users -->|M| Bill
        Users -->|1| billHistory
        Users -->|1| SearchHistory
        Users -->|1| ShoppingCart
        Users -->|1| Subscriptions
        Wallet -->|1| Bill
        Bill -->|1| billHistory
        Bill -->|1| SearchHistory
        Bill -->|M| ProductList
        Bill -->|M| Promocode
        Bill -->|M| Reviews
        billHistory -->|M| Promocode
        ProductItem -->|M| ProductList
        ProductItem -->|M| Categories
        ProductItem -->|M| Brands
        Reviews -->|M| ProductItem
        ShoppingCart -->|1| CartItems
        CartItems -->|1| ProductItem
        Subscriptions -->|1| Plans
        Brands -->|M| Categories
        Brands -->|M| ProductList
        Brands -->|M| Reviews
        Brands -->|1| About
    end
