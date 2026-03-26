import 'package:flutter/material.dart';
import 'services/websocket_service.dart';

void main() {
  runApp(const MyApp());
}

class WebSocketPage extends StatefulWidget {
  const WebSocketPage({super.key});

  @override
  State<WebSocketPage> createState() => _WebSocketPageState();
}

class _WebSocketPageState extends State<WebSocketPage> {
  final ws = WebSocketService('ws://localhost:8080/ws');

  @override
  void initState() {
    super.initState();

    ws.connect();

    ws.stream.listen((message) {
      print('Received: $message');
    });
  }

  @override
  void dispose() {
    ws.disconnect();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('WebSocket Demo')),
      body: Center(
        child: ElevatedButton(
          onPressed: () {
            ws.send('Hello from Flutter');
          },
          child: const Text('Send Message'),
        ),
      ),
    );
  }
}

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
