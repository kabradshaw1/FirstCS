import 'package:web_socket_channel/web_socket_channel.dart';

class WebSocketService {
  final String url;
  WebSocketChannel? _channel;

  WebSocketService(this.url);

  /// Connect to the WebSocket
  void connect() {
    _channel = WebSocketChannel.connect(Uri.parse(url));
  }

  /// Stream of incoming messages
  Stream get stream {
    if (_channel == null) {
      throw Exception('WebSocket not connected');
    }
    return _channel!.stream;
  }

  /// Send a message
  void send(String message) {
    if (_channel == null) {
      throw Exception('WebSocket not connected');
    }
    _channel!.sink.add(message);
  }

  /// Close the connection
  void disconnect() {
    _channel?.sink.close();
    _channel = null;
  }
}
