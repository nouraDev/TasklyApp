import { Link } from "react-router-dom"
import { Button } from "../../components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "../../components/ui/card"

export function Welcome() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100/40 dark:bg-gray-900/40">
      <Card className="w-[600px] text-center">
        <CardHeader>
          <CardTitle className="text-4xl">Welcome to Taskly plateform</CardTitle>
          <CardDescription className="text-lg">
            Organize your tasks efficiently and boost your productivity
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-6">
          <p className="text-muted-foreground">
            Get started by creating an account or logging in to access your tasks
          </p>
          <div className="flex justify-center gap-4">
            <Button asChild size="lg">
              <Link
                to="/register"
                className="px-6 py-3 bg-indigo-600 text-indigo-600 rounded-md text-lg font-medium hover:bg-indigo-700 transition-colors duration-200"
              >
                Get Started
              </Link>
            </Button>
            <Button asChild size="lg">
              <Link
                to="/login"
                className="px-6 py-3 bg-indigo-600 text-indigo-600 rounded-md text-lg font-medium hover:bg-indigo-700 transition-colors duration-200"
              >
                Sign In
              </Link>
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}
