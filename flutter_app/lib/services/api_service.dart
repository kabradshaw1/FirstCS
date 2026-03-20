import 'dart:convert';

import 'package:http/http.dart' as http;

import '../models/user.dart';

class ApiService {
  final String baseUrl;
  final http.Client client;

  ApiService({required this.baseUrl, http.Client? client})
    : client = client ?? http.Client();

  Future<List<User>> getUsers() async {
    final uri = Uri.parse('$baseUrl/users');

    try {
      final response = await http.get(uri).timeout(const Duration(seconds: 10));

      if (response.statusCode != 200) {
        throw Exception(
          'Failed to load users (status ${response.statusCode}): ${response.body}',
        );
      } else {
        final decoded = jsonDecode(response.body);

        if (decoded is! List) {
          throw Exception('Expected a list but got: $decoded');
        }

        return decoded.map((item) {
          if (item is! Map<String, dynamic>) {
            throw FormatException('Invalid user item: $item');
          }
          return User.fromJson(item);
        }).toList();
      }
    } catch (e) {
      throw Exception('Error fetching users: $e');
    }
  }

  Future<User> createUser(String name, int age) async {
    final uri = Uri.parse('$baseUrl/users');

    final response = await http.post(
      uri,
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'name': name, 'age': age}),
    );

    if (response.statusCode == 201) {
      final Map<String, dynamic> json =
          jsonDecode(response.body) as Map<String, dynamic>;
      return User.fromJson(json);
    } else {
      throw Exception('Failed to create user (status ${response.statusCode})');
    }
  }
}
