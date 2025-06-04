# 🛒 Inventory-Shop System (Unity)

**Inventory-Shop System** is a modular, scalable, and event-driven framework for managing in-game inventory and shop interactions. Built entirely in **Unity (C#)**, this system is designed to integrate into any Unity project with minimal effort, providing seamless item management, UI feedback, and powerful gameplay systems.

---

## ✨ Features Overview

### 📦 Dynamic Inventory Management
- Track **item quantity** and **weight capacity** in real-time.
- Spawn items dynamically with **randomized quantities** and weight validation.
- UI updates **instantly** to reflect changes in inventory.

### 🛍️ Interactive Shop System
- Buy and sell items with full **transaction management**.
- Real-time **quantity and price updates**.
- Handles edge cases like **insufficient coins** or **max weight limit exceeded**.
- Displays item details in sleek, dynamic **UI panels**.

### 🔁 Robust Event-Driven Architecture
- Central **EventService** orchestrates item and UI interactions.
- Observer Pattern ensures all UI and logic systems respond to changes.
- Audio, inventory, and UI elements react immediately to in-game events.

### 🧱 Modular & Scalable Design
- Built on a clean **MVC architecture** with reusable **generic classes**.
- Shared base scripts across Inventory and Shop using **inheritance**.
- **ScriptableObjects** define item properties for easy content expansion.

### 🎮 Enhanced User Experience
- Toggle between **Inventory** and **Shop** views effortlessly.
- UI elements controlled with **CanvasGroup** for polished interactivity.
- Real-time **sound feedback** using a centralized `SoundManager` system.
- Interactive **pop-ups** alert users for weight limits, item details, and funds.

---

## 💡 Technologies & Architecture

### 🧩 Design Patterns
- **Observer Pattern** for real-time UI & logic updates.
- **MVC Pattern** separates data (Model), logic (Controller), and visuals (View).
- **Generic Singleton** for managing core services like Sound and Event systems.

### 💻 Advanced C# Features
- Used **Actions**, **delegates**, and **events** to handle inter-system communication.
- **Generic controllers** and **interfaces** for reusability and flexibility.
- Built a custom `GenericMonoSingleton<T>` base class for global systems.

### 🎨 UI/UX Techniques
- Toggle views with **smooth transitions**.
- Pop-up prompts for **error messages** and **transaction confirmations**.
- Custom transaction UI with real-time **buying/selling sections** and input validation.

---

## 🛠️ Development Approach

- **Agile & Iterative**: Designed with configurable settings via Unity Inspector.
- **Extensible & Maintainable**: Easily add new items, item types, or UI elements.
- **Optimized Performance**: Event-driven updates eliminate unnecessary polling or lag.

---

## Play Link
https://sayannandi.itch.io/inventory-shop-system

[![Watch the video](https://img.youtube.com/vi/x6bbBza0w_4/maxresdefault.jpg)](https://youtu.be/x6bbBza0w_4)
### [Gameplay Video](https://youtu.be/x6bbBza0w_4)

![Image](https://github.com/user-attachments/assets/cb40eef5-947d-4257-8476-770d5f1b1db6)

![Image](https://github.com/user-attachments/assets/15aceaba-9943-4bee-9f79-92191dccd581)

![Image](https://github.com/user-attachments/assets/1dcef6ad-ddf8-4efc-bb8a-c77b24ee6449)

![Image](https://github.com/user-attachments/assets/09ef28fd-f6d0-4f8c-9b81-a6f325b18b0b)

![Image](https://github.com/user-attachments/assets/d4143c92-8d67-46d8-aad3-301af9852dd3)

![Image](https://github.com/user-attachments/assets/e5eec387-6a7a-4cfd-b4a3-091760163d74)

![Image](https://github.com/user-attachments/assets/858e4c26-9bad-4063-91d3-71c27fd2dcbd)

![Image](https://github.com/user-attachments/assets/4e3866c0-ea7c-4c3b-b4a9-1036a9fa54af)

![Image](https://github.com/user-attachments/assets/642e7458-4051-4564-a5df-2878b7faa0eb)

![Image](https://github.com/user-attachments/assets/72de53d2-cbcf-470b-9633-4954971467ed)
