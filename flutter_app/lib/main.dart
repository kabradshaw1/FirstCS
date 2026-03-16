// main.dart — the entry point for every Flutter application.
//
// Flutter works by building a tree of "widgets". A widget is just a Dart class
// that describes a piece of UI. When the app starts, Flutter calls runApp()
// with the root widget and renders the tree to the screen.

// This import pulls in Flutter's Material Design widget library.
// "Material" means Google's design system (buttons, text fields, app bars, etc.)
import 'package:flutter/material.dart';

// main() is the Dart entry point, just like `static void Main()` in C#.
// The `void` return type means it returns nothing.
void main() {
  // runApp() inflates the widget you pass it and attaches it to the screen.
  // Everything in Flutter starts here.
  runApp(const MyApp());
}

// StatelessWidget is the base class for widgets that never change after they
// are built. Think of it like a pure function — same inputs always produce
// the same output. There is also StatefulWidget for widgets that can change
// (we'll use that later when we display real data from the API).
//
// The `const` constructor tells Dart that this object's values are known at
// compile time and will never change — a small performance optimisation.
class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // build() is the one method every widget must implement.
  // Flutter calls it whenever it needs to draw (or redraw) this widget.
  // `BuildContext` carries information about where in the widget tree we are.
  @override
  Widget build(BuildContext context) {
    // MaterialApp is the top-level widget that wires up:
    //   • a Navigator (for routing between screens)
    //   • a Theme (colours, fonts, etc.)
    //   • localisation helpers
    // You only need one MaterialApp per application.
    return MaterialApp(
      // `title` shows up in the OS task-switcher.
      title: 'FirstC Flutter App',

      // `theme` lets us customise colours across the whole app.
      // We'll expand this later; for now we just pick a seed colour.
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.blue),
        useMaterial3: true, // opt into Material 3 (the latest design spec)
      ),

      // `home` is the widget shown at the root route ("/").
      // Right now it's just a blank scaffold — we'll replace this with a
      // real screen once the model and service layers are wired up.
      home: const PlaceholderScreen(),
    );
  }
}

// PlaceholderScreen is a temporary home screen.
// It will be replaced by a UsersScreen in the next increment.
class PlaceholderScreen extends StatelessWidget {
  const PlaceholderScreen({super.key});

  @override
  Widget build(BuildContext context) {
    // Scaffold provides the basic page structure:
    //   • AppBar  — the top bar
    //   • body    — the main content area
    //   • floatingActionButton, bottomNavigationBar, drawer, etc.
    return Scaffold(
      appBar: AppBar(
        // We read the primary colour from the current Theme rather than
        // hard-coding a colour — this keeps the UI consistent everywhere.
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        title: const Text('SecondApi Demo'),
      ),
      // Center positions its single child in the middle of the available space.
      body: const Center(
        // Text is the simplest widget for displaying a string.
        child: Text('Flutter app ready — UI coming soon!'),
      ),
    );
  }
}
