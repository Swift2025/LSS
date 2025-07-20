// src/app/interfaces/chat.interface.ts

/**
 * Represents a single message in the chat interface.
 */
export interface ChatMessage {
  id: number;
  text: string;
  sender: 'user' | 'assistant' | 'system' | 'error'; // Added 'error' sender type
  timestamp: Date;
  status?: 'sending' | 'sent';
}

/**
 * Represents the request sent to the Django backend.
 */
export interface SupportRequest {
  query: string;
  channel_name: string;
}

/**
 * Represents the initial response from the Django backend.
 */
export interface SupportResponse {
  initial_response: string;
  intent: string;
  entities: {
    apps?: string[];
    environment?: string;
  };
}

/**
 * Represents a progress update message from the WebSocket.
 */
export interface ProgressUpdate {
  message: string;
  percentage_complete: number;
  status: 'IN_PROGRESS' | 'COMPLETED' | 'FAILED';
  error?: {
    code: string;
    message: string;
  };
}
