import React from 'react';

const ContactPage: React.FC = () => {
    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        // Handle form submission logic
        alert("Thank you for your message! We'll get back to you soon.");
    };

    return (
        <div className="bg-light py-12">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="text-center">
                    <h1 className="text-4xl font-extrabold text-secondary">Get in Touch</h1>
                    <p className="mt-2 text-lg text-gray-600">We'd love to hear from you. Send us a message!</p>
                </div>

                <div className="mt-12 lg:grid lg:grid-cols-2 lg:gap-8 items-start">
                    {/* Contact Info */}
                    <div className="bg-white p-8 rounded-lg shadow-md">
                        <h2 className="text-2xl font-bold text-secondary mb-6">Contact Information</h2>
                        <div className="space-y-4 text-gray-700">
                            <p><strong>Address:</strong> 123 Pizza Street, Flavor Town, 12345</p>
                            <p><strong>Phone:</strong> (555) 123-4567</p>
                            <p><strong>Email:</strong> contact@pizzaorders.com</p>
                        </div>
                        <div className="mt-6 h-64 bg-gray-200 rounded-lg">
                            {/* Placeholder for a map */}
                             <iframe 
                                src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3153.085888283038!2d-122.4194154846813!3d37.77492957975904!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x8085808c1b2b3c2b%3A0x8a3f7f8d6f5f3e3a!2sSan%20Francisco%2C%20CA!5e0!3m2!1sen!2sus!4v1620930143821!5m2!1sen!2sus" 
                                width="100%" 
                                height="100%" 
                                style={{border:0}} 
                                allowFullScreen={true}
                                loading="lazy"
                                title="Location Map">
                            </iframe>
                        </div>
                    </div>

                    {/* Contact Form */}
                    <div className="mt-8 lg:mt-0 bg-white p-8 rounded-lg shadow-md">
                        <form onSubmit={handleSubmit} className="space-y-6">
                            <div>
                                <label htmlFor="name" className="block text-sm font-medium text-gray-700">Name</label>
                                <input type="text" id="name" required className="mt-1 block w-full px-3 py-2 bg-white border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-primary focus:border-primary"/>
                            </div>
                            <div>
                                <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
                                <input type="email" id="email" required className="mt-1 block w-full px-3 py-2 bg-white border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-primary focus:border-primary"/>
                            </div>
                            <div>
                                <label htmlFor="message" className="block text-sm font-medium text-gray-700">Message</label>
                                <textarea id="message" rows={4} required className="mt-1 block w-full px-3 py-2 bg-white border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-primary focus:border-primary"></textarea>
                            </div>
                            <div>
                                <button type="submit" className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-primary hover:bg-primary-dark focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary">
                                    Send Message
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ContactPage;