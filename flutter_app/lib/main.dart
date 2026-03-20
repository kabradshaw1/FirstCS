import 'package:web_socket_channel/web_socket_channel.dart';
import 'package:flutter/material.dart';

void main() {
  runApp(const MyApp());
}

final channel = WebSocketChannel.connect(Uri.parse('ws://localhost:8080/ws'));

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'FirstC Flutter App',

      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.blue),
        useMaterial3: true, // opt into Material 3 (the latest design spec)
      ),

      home: const PlaceholderScreen(),
    );
  }
}

class PlaceholderScreen extends StatelessWidget {
  const PlaceholderScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        title: const Text('SecondApi Demo'),
      ),

      body: const Center(child: Text('Flutter app ready — UI coming soon!')),
    );
  }
}
